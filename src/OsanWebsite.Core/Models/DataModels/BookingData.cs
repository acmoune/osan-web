using OsanWebsite.Core.Dtos;

namespace OsanWebsite.Core.Models.DataModels;

public class BookingData
{
    public string id { get; set; } = default!;
    public BookingDataAttributes attributes { get; set; } = default!;

    public Booking ToBooking()
    {
        var dto = new BookingDto
        {
            Id = this.id,
            CustomerName = this.attributes.CustomerName,
            CustomerPhone = this.attributes.CustomerPhone,
            CustomerEmail = this.attributes.CustomerEmail,
        };

        return new Booking
        (
            bookingDto: dto,
            bookingId: Guid.Parse(this.attributes.BookingId),
            campaign: this.attributes.booking_campaign.data.ToBookingCampaign(),
            tableType: this.attributes.table_type.data.ToTableType(),
            qrCodeUrl: this.attributes.QrCodeUrl,
            status: this.attributes.Status,
            cancellationReason: this.attributes.CancellationReason
        ) ;
    }
}

public class BookingDataAttributes
{
    public string BookingId { get; set; } = default!;
    public string CustomerName { get; set; } = default!;
    public string CustomerPhone { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public string? QrCodeUrl { get; set; }
    public string? CancellationReason { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public BookingCampaignItemData booking_campaign { get; set; } = default!;
    public TableTypeItemData table_type { get; set; } = default!;
}

public class BookingCollectionData
{
    public IEnumerable<BookingData> data { get; set; } = default!;
    public PaginationData? meta { get; set; }
}

public class BookingItemData
{
    public BookingData data { get; set; } = default!;
}

public class BookingCollectionResponse
{
    public BookingCollectionData bookings { get; set; } = default!;
}
