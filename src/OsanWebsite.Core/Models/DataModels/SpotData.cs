namespace OsanWebsite.Core.Models.DataModels;

public class SpotData
{
    public string id { get; set; } = default!;
    public SpotAttributes attributes { get; set; } = default!;

    public Spot ToSpot()
    {
        var img = this.attributes.Image.data != null ? this.attributes.Image.data.attributes.url : null;

        return new Spot
        {
            Id = this.id,
            Slug = this.attributes.Slug,
            Title = this.attributes.Title,
            Date = this.attributes.Date,
            IsVideo = this.attributes.IsVideo,
            ImageUrl = img,
            YoutubeVideoId = this.attributes.YoutubeVideoId,
            Description = this.attributes.Description,
            Keywords = this.attributes.Keywords,
            Body = this.attributes.Body,
        };
    }
}

public class SpotAttributes
{
    public string Slug { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateOnly Date { get; set; } = default!;
    public bool IsVideo { get; set; }
    public ImageData Image { get; set; } = default!;
    public string? YoutubeVideoId { get; set; }
    public string? Description { get; set; }
    public string? Keywords { get; set; }
    public string? Body { get; set; }
}

public class SpotCollectionData
{
    public IEnumerable<SpotData> data { get; set; } = default!;
    public PaginationData? meta { get; set; }
}

public class SpotCollectionResponse
{
    public SpotCollectionData spots { get; set; } = default!;
}
