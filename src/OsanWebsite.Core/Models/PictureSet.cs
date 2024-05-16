namespace OsanWebsite.Core.Models;

public class PictureSet
{
    public string? Id { get; set; }
    public string Name { get; set; } = default!;
    public IEnumerable<Picture> Pictures { get; set; } = default!;
}
