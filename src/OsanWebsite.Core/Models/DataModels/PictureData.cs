namespace OsanWebsite.Core.Models.DataModels;

public class PictureData
{
    public string id { get; set; } = default!;
    public PictureAttributes attributes { get; set; } = default!;

    public Picture ToPicture()
    {
        return new Picture
        {
            Id = this.id,
            Title = this.attributes.Title,
            ImageUrl = this.attributes.Image.data.attributes.url,
        };
    }
}

public class PictureAttributes
{
    public string Title { get; set; } = default!;
    public ImageData Image { get; set; } = default!;
}

public class PictureCollectionData
{
    public IEnumerable<PictureData> data { get; set; } = default!;
}

public class PictureSetAttributes
{
    public PictureCollectionData pictures { get; set; } = default!;
}

public class PictureSetData
{
    public string id { get; set; } = default!;
    public PictureSetAttributes attributes { get; set; } = default!;

    public PictureSet ToPictureSet(string name)
    {
        return new PictureSet
        {
            Id = this.id,
            Name = name,
            Pictures = this.attributes.pictures.data.Select(d => d.ToPicture()),
        };
    }
}

public class PictureSetCollectionData
{
    public IEnumerable<PictureSetData> data { get; set; } = default!;
}

public class PictureSetResponse
{
    public PictureSetCollectionData pictureSets { get; set; } = default!;
}
