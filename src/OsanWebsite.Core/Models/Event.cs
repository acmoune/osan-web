using OsanWebsite.Core.Helpers;

namespace OsanWebsite.Core.Models;

public class Event : INews
{
    public string? Id { get; set; }
    public string Slug { get; set; } = default!;
    public string Title { get; set; } = default!;
    public Service Service { get; set; } = default!;
    public DateOnly Date { get; set; } = default!;
    public bool IsVideo { get; set; }
    public string? ImageUrl { get; set; } = default!;
    public string? YoutubeVideoId { get; set; }
    public bool Archived { get; set; } = false;
    public string? Description { get; set; }
    public string? Keywords { get; set; }
    public string? Body { get; set; }

    public string GetImageUrl() => IsVideo ? MediaHelpers.YoutubeVideoThumbnail(YoutubeVideoId!) : ImageUrl!;
    public string GetUrl() => $"/events/{Slug}";
}
