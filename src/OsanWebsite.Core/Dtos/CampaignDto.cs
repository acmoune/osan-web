namespace OsanWebsite.Core.Dtos;

public class CampaignDto
{
    public DateOnly BookingDate { get; set; } = default!;
    public string ServiceName { get; set; } = default!;
}
