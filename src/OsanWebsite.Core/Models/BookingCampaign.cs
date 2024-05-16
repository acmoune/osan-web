namespace OsanWebsite.Core.Models;

public record BookingCampaign (string? Id, DateOnly BookingDate, Service Service, BookingCampaignStatus Status)
{
    public string Name() => $"{BookingDate.ToString("dd-MM-yy")}_{Service.Name.Replace(" ", "-")}";
    public string DisplayName() => $"{BookingDate.ToString("dd-MM-yy")}_{Service.Name}";
}

public enum BookingCampaignStatus
{
    Opened,
    Closed,
    Completed
}
