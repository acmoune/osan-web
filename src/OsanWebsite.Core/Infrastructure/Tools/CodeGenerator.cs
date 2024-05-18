using QRCoder;

namespace OsanWebsite.Core.Infrastructure.Tools;

public static class CodeGenerator
{
    public static MemoryStream CreateQrCodePngStream(string code)
    {
        var generator = new QRCoder.QRCodeGenerator();
        var data = generator.CreateQrCode(code, QRCoder.QRCodeGenerator.ECCLevel.L);
        var png = new PngByteQRCode(data);
        var dataBytes = png.GetGraphic(20);
        return new MemoryStream(dataBytes);
    }
}
