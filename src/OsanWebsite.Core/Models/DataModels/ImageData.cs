namespace OsanWebsite.Core.Models.DataModels;

public class ImageObject
{
    public string url { get; set; } = default!;
}

public class ImageResponse
{
    public ImageObject attributes { get; set; } = default!;
}

public class ImageData
{
    public ImageResponse data { get; set; } = default!;
}

public class NullableImageData
{
    public ImageResponse? data { get; set; }
}
