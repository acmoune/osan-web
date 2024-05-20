namespace OsanWebsite.Core.Models;

public record Service (string? Id, string Name, string? Description, TimeOnly StartTime, TimeOnly EndTime)
{
    public string ToHtmlString()
    {
        return $"<strong>{Name}</strong> . {StartTime.ToString("HH:mm")} - {EndTimeString()}";
    }

    public string EndTimeString() => EndTime.ToString("HH:mm") == "00:00" ? "Fermeture" : EndTime.ToString("HH:mm");
}
