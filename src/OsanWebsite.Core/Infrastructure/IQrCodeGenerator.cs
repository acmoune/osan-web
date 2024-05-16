namespace OsanWebsite.Core.Infrastructure;

public interface IQrCodeGenerator
{
    Task<QrCodeInfo> GenerateFrom(string text);
}

public record QrCodeInfo(string Url);
