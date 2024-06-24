namespace OsanWebsite.Core.Models.DataModels;

public class BookingCampaignData
{
    public string id { get; set; } = default!;
    public BookingCampaignAttributes attributes { get; set; } = default!;

    public BookingCampaign ToBookingCampaign()
    {
        return new BookingCampaign
        (
            Id: this.id,
            BookingDate: this.attributes.BookingDate,
            Service: this.attributes.service.data.ToService(),
            Status: this.attributes.Status
        );
    }
}

public class BookingCampaignAttributes
{
    public DateOnly BookingDate {  get; set; } = default!; 
    public ServiceItemData service { get; set; } = default!; 
    public BookingCampaignStatus Status { get; set; } = default!;
}

public class BookingCampaignCollectionData
{
    public IEnumerable<BookingCampaignData> data { get; set; } = default!;
}

public class BookingCampaignItemData
{
    public BookingCampaignData data { get; set; } = default!;
}

public class BookingCampaignCollectionResponse
{
    public BookingCampaignCollectionData bookingCampaigns { get; set; } = default!;
}
