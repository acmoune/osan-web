namespace OsanWebsite.Core.Models.DataModels;

public class EventData
{
    public string id { get; set; } = default!;
    public EventAttributes attributes { get; set; } = default!;

    public Event ToEvent()
    {
        var img = this.attributes.Image.data != null ? this.attributes.Image.data.attributes.url : null;

        return new Event
        {
            Id = this.id,
            Title = this.attributes.Title,
            Slug = this.attributes.Slug,
            Date = this.attributes.Date,
            Archived = this.attributes.Archived,
            Description = this.attributes.Description,
            Keywords = this.attributes.Keywords,
            IsVideo = this.attributes.IsVideo,
            ImageUrl = img,
            YoutubeVideoId = this.attributes.YoutubeVideoId,
            Body = this.attributes.Body,
            Service = new Service
            (
                Id: this.attributes.service.data.id,
                Name: this.attributes.service.data.attributes.Name,
                Description: this.attributes.service.data.attributes.Description,
                StartTime: this.attributes.service.data.attributes.StartTime,
                EndTime: this.attributes.service.data.attributes.EndTime
            )
        };
    }
}

public class EventAttributes
{
    public string Slug { get; set; } = default!;
    public string Title { get; set; } = default!;
    public ServiceItemData service { get; set; } = default!;
    public DateOnly Date { get; set; } = default!;
    public bool IsVideo { get; set; }
    public ImageData Image { get; set; } = default!;
    public string? YoutubeVideoId { get; set; }
    public bool Archived { get; set; } = false;
    public string? Description { get; set; }
    public string? Keywords { get; set; }
    public string? Body { get; set; }
}

public class EventCollectionData
{
    public IEnumerable<EventData> data { get; set; } = default!;
    public PaginationData? meta { get; set; }
}

public class EventCollectionResponse
{
    public EventCollectionData events { get; set; } = default!;
}
