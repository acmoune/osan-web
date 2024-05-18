namespace OsanWebsite.Core.Infrastructure.Tools;

public static class FileDownloader
{
    public static async Task<byte[]> DownloadAsBytes(string url)
    {
        using (HttpClient http = new HttpClient())
        {
            var myBytes = await http.GetByteArrayAsync(url);
            return myBytes;
        }
    }
}
