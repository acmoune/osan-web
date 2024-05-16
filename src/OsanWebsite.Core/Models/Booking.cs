using OsanWebsite.Core.Dtos;

namespace OsanWebsite.Core.Models;

public class Booking
{
    public string? Id { get; set; }
    public Guid BookingId { get; private set; } = default!;
    public string CustomerName { get; private set; } = default!;
    public string CustomerPhone { get; private set; } = default!;
    public string CustomerEmail { get; private set; } = default!;
    public BookingCampaign Campaign { get; private set; } = default!;
    public TableType TableType { get; private set; } = default!;
    public string? QrCodeUrl { get; private set; }
    public string? CancellationReason { get; private set; }
    public BookingStatus Status { get; private set; } = BookingStatus.Pending;

    public Booking(BookingDto bookingDto, Guid bookingId, BookingCampaign campaign, TableType tableType, string? qrCodeUrl = null, string? cancellationReason = null, BookingStatus status = BookingStatus.Pending)
    {
        Id = bookingDto.Id;
        BookingId = bookingId;
        CustomerName = bookingDto.CustomerName;
        CustomerPhone = bookingDto.CustomerPhone;
        CustomerEmail = bookingDto.CustomerEmail;
        Campaign = campaign;
        TableType = tableType;
        QrCodeUrl = qrCodeUrl;
        Status = status;
        CancellationReason = cancellationReason;
    }

    public void SetStatus(BookingStatus status)
    {
        Status = status;
    }

    public void SetCustomerDetails(string name, string email, string phone)
    {
        CustomerName = name;
        CustomerPhone = phone;
        CustomerEmail = email;
    }

    public void SetTableType(TableType tableType)
    {
        TableType = tableType;
    }

    public void SetCancellationReason(string reason)
    {
        CancellationReason = reason;
    }

    public void SetQrCodeUrl(string qrCodeUrl)
    {
        QrCodeUrl = qrCodeUrl;
    }
}

public enum BookingStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Consumed,
    Unconsumed,
}
