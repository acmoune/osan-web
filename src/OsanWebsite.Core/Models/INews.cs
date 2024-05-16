namespace OsanWebsite.Core.Models;

public interface INews
{
    string Title { get; set; }
    DateOnly Date { get; set; }

    string GetImageUrl();
    string GetUrl();
}
