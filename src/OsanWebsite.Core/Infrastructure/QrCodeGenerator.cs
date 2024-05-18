using OsanWebsite.Core.Infrastructure.Tools;

namespace OsanWebsite.Core.Infrastructure;

public class QrCodeGenerator : IQrCodeGenerator
{
    public async Task<QrCodeInfo> GenerateFrom(string text)
    {
        var fileName = $"{text}_{Guid.NewGuid().ToString()}.png";

        var memoryStream = CodeGenerator.CreateQrCodePngStream(text);
        await S3Uploader.Upload(memoryStream, fileName);

        return new QrCodeInfo($"https://osan-codes.s3.us-west-2.amazonaws.com/{fileName}");
    }
}
