using OsanWebsite.Core.Dtos;
using OsanWebsite.Core.Models;
using OsanWebsite.Core.Repositories;
using OsanWebsite.Core.Services;
using OsanWebsite.Web.Components.Admin.Infra;


namespace OsanWebsite.Web.Components.Admin;

public class DataService : BaseViewModel
{
    private IBookingRepository _repo;
    private readonly IBookingService _bookingService;

    public DataService(IBookingRepository repo, IBookingService bookingService)
    {
        _repo = repo;
        _bookingService = bookingService;
    }

    // Properties

    private IEnumerable<BookingCampaign>? _campaigns = default!;
    public IEnumerable<BookingCampaign>? Campaigns
    {
        get => _campaigns;
        set => SetValue(ref _campaigns, value);
    }

    private BookingCampaign? _selectedCampaign;
    public BookingCampaign? SelectedCampaign
    {
        get => _selectedCampaign;
        set => SetValue(ref _selectedCampaign, value);
    }

    private PaggingResult<BookingItem>? _bookings;
    public PaggingResult<BookingItem>? Bookings
    {
        get => _bookings;
        set => SetValue(ref _bookings, value);
    }

    private Booking? _booking;
    public Booking? SelectedBooking
    {
        get => _booking;
        set => SetValue(ref _booking, value);
    }


    // Operations

    public async Task FetchCampaigns(int year, int month)
    {
        SelectedCampaign = null;
        SelectedBooking = null;
        Campaigns = await _repo.GetCampaigns(year, month);
    }

    public async Task FetchBookings(int page, int size)
    {
        if (SelectedCampaign != null)
        {
            SelectedBooking = null;
            Bookings = await _repo.GetBookings(SelectedCampaign.Id!, page, size);
        }
    }

    public async Task AddBookingThunk(BookingDto dto)
    {
        await _bookingService.CreateBooking(dto);
        await FetchBookings(1, 10);
    }

    public async Task UpdateBookingThunk(Guid id, BookingDto dto)
    {
        var booking = await _bookingService.UpdateBooking(id, dto.CustomerName, dto.CustomerEmail, dto.CustomerPhone, dto.TableType!);
        UpdateBooking(new BookingItem(booking));
    }

    public async Task ConfirmBookingThunk(Guid id)
    {
        var item = Bookings!.Items.Where(i => i.Booking.BookingId == id).Single();
        item.Confirming = true;
        UpdateBooking(item);

        var booking = await _bookingService.ConfirmBooking(id);

        UpdateBooking(new BookingItem(booking));
    }

    public async Task CancelBookingThunk(Guid id)
    {
        var item = Bookings!.Items.Where(i => i.Booking.BookingId == id).Single();
        item.Cancelling = true;
        UpdateBooking(item);

        var booking = await _bookingService.CancelBooking(id, "User cancelled");

        UpdateBooking(new BookingItem(booking));
    }

    public async Task ConsumeBookingThunk(Guid id)
    {
        var item = Bookings!.Items.Where(i => i.Booking.BookingId == id).Single();
        item.Consuming = true;
        UpdateBooking(item);

        var booking = await _bookingService.MarkAsConsumed(id);

        UpdateBooking(new BookingItem(booking));
    }

    public async Task CloseCampaignThunk(string campaign_id)
    {
        var campaign = await _bookingService.CloseCampaign(campaign_id);
        await FetchCampaigns(campaign.BookingDate.Year, campaign.BookingDate.Month);
    }

    public async Task CompleteCampaignThunk(string campaign_id)
    {
        var campaign = await _bookingService.CompleteCampaign(campaign_id);
        await FetchCampaigns(campaign.BookingDate.Year, campaign.BookingDate.Month);
    }

    private void UpdateBooking(BookingItem item)
    {
        if (Bookings != null)
        {
            var newItems = Bookings.Items.Select(b => (b.Booking.BookingId != item.Booking.BookingId) ? b : item);
            Bookings = new PaggingResult<BookingItem>(newItems, Bookings.TotalItems, Bookings.PageSize, Bookings.TotalPages, Bookings.CurrentPage);
        }
    }
}
