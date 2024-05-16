namespace OsanWebsite.Core.Models;

public class Picture
{
    public string? Id { get; set; }
    public string Title { get; init; } = default!;
    public string ImageUrl { get; init; } = default!;
}
