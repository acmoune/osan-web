namespace OsanWebsite.Core.Helpers;

public static class MediaHelpers
{
    public static string YoutubeVideoLink(string VideoId) => $"https://www.youtube.com/embed/{VideoId}";

    public static string YoutubeVideoThumbnail(string VideoId) => $"https://img.youtube.com/vi/{VideoId}/maxresdefault.jpg";
}
