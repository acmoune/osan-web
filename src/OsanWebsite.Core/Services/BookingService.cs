using OsanWebsite.Core.Dtos;
using OsanWebsite.Core.Models;
using OsanWebsite.Core.Exceptions;
using OsanWebsite.Core.Repositories;
using OsanWebsite.Core.Events;
using MediatR;
using OsanWebsite.Core.Infrastructure;
using Microsoft.Extensions.Logging;

namespace OsanWebsite.Core.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _repo;
    private readonly IMediator _mediator;
    private IQrCodeGenerator _qrCodeGenerator;
    private readonly ILogger<BookingService> _logger;

    public BookingService(IBookingRepository repo, IMediator mediator, IQrCodeGenerator qrCodeGenerator, ILogger<BookingService> logger)
    {
        _repo = repo;
        _mediator = mediator;
        _qrCodeGenerator = qrCodeGenerator;
        _logger = logger;
    }

    public async Task<Booking> CreateBooking(BookingDto dto)
    {
        var tableType = await _repo.GetTableTypeByBame(dto.TableType!);
        var service = await _repo.GetServiceByBame(dto.Service!);

        var campaign = await GetOrCreateCampaign(DateOnly.FromDateTime((DateTime)dto.BookingDate!), service);

        if (campaign.Status != BookingCampaignStatus.Opened)
        {
            throw new BusinessRuleValidationException("Campaign not opened for booking");
        }

        var booking = new Booking(dto, Guid.NewGuid(), campaign, tableType);
        var savedBooking = await _repo.Create(booking);
        await _mediator.Publish(new BookingCreatedEvent { Booking = savedBooking });
        return savedBooking;
    }

    public async Task<Booking> UpdateBooking(Guid bookingId, string name, string email, string phone, string tableTypeName)
    {
        var booking = await _repo.GetById(bookingId);
        var tableType = await _repo.GetTableTypeByBame(tableTypeName);

        booking.SetCustomerDetails(name, email, phone);
        booking.SetTableType(tableType);

        var savedBooking = await _repo.Save(booking);

        return savedBooking;
    }

    public async Task<Booking> CancelBooking(Guid bookingId, string reason)
    {
        var booking = await _repo.GetById(bookingId);
        if (booking.Status != BookingStatus.Pending)
        {
            throw new BusinessRuleValidationException("Can only cancel a pending booking");
        }
        booking.SetStatus(BookingStatus.Cancelled);
        booking.SetCancellationReason(reason);
        var savedBooking = await _repo.Save(booking);
        await _mediator.Publish(new BookingCancelledEvent { Booking = savedBooking });
        return savedBooking;
    }

    public async Task<Booking> ConfirmBooking(Guid bookingId)
    {
        var booking = await _repo.GetById(bookingId);
        if (booking.Status != BookingStatus.Pending)
        {
            throw new BusinessRuleValidationException("Can only confirm a pending booking");
        }

        try
        {
            var qrCodeInfo = await _qrCodeGenerator.GenerateFrom(booking.BookingId.ToString());
            _logger.LogInformation($"Successfully uploaded QrCode png to S3 [{qrCodeInfo.Url}]");
            booking.SetQrCodeUrl(qrCodeInfo.Url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
        
        booking.SetStatus(BookingStatus.Confirmed);
        
        var savedBooking = await _repo.Save(booking);
        await _mediator.Publish(new BookingConfirmedEvent { Booking = savedBooking });
        return savedBooking;
    }

    public async Task<Booking> MarkAsConsumed(Guid bookingId)
    {
        var booking = await _repo.GetById(bookingId);

        if (booking.Status == BookingStatus.Consumed)
        {
            // QR Code already scanner on another table member.
            return booking;
        }

        if (booking.Status != BookingStatus.Confirmed)
        {
            throw new BusinessRuleValidationException("Can only consume a confirmed booking");
        }

        booking.SetStatus(BookingStatus.Consumed);
        var savedBooking = await _repo.Save(booking);
        return savedBooking;
    }

    public async Task<BookingCampaign> GetOrCreateCampaign(DateOnly bookingDate, Service service)
    {
        var campaign = await _repo.GetCampaign(bookingDate, service.Name);

        if (campaign != null)
        {
            return campaign;
        }

        if (bookingDate < DateOnly.FromDateTime(DateTime.Now))
        {
            throw new BusinessRuleValidationException("Can not book on a past date");
        }

        var newCampaign = new BookingCampaign(null, bookingDate, service, BookingCampaignStatus.Opened);
        var savedCampaign = await _repo.SaveCampaign(newCampaign);
        return savedCampaign;
    }

    public async Task<BookingCampaign> CloseCampaign(string campaign_id)
    {
        var campaign = await _repo.GetCampaign(campaign_id);
        if (campaign!.Status != BookingCampaignStatus.Opened)
        {
            throw new BusinessRuleValidationException("Can only close an opened campaign");
        }
        campaign = campaign with { Status = BookingCampaignStatus.Closed };
        return await _repo.SaveCampaign(campaign);
    }

    public async Task<BookingCampaign> CompleteCampaign(string campaign_id)
    {
        var campaign = await _repo.GetCampaign(campaign_id);
        if (campaign!.Status != BookingCampaignStatus.Closed)
        {
            throw new BusinessRuleValidationException("Can only complete a closed campaign");
        }
        campaign = campaign with { Status = BookingCampaignStatus.Completed };
        var savedCampaign = await _repo.SaveCampaign(campaign);
        await _repo.CompleteCampaign(campaign);
        return savedCampaign;
    }
}
