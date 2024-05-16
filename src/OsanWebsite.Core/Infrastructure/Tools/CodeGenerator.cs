using IronBarCode;

namespace OsanWebsite.Core.Infrastructure.Tools;

public static class CodeGenerator
{
    public static MemoryStream CreateQrCodePngStream(string code)
    {
        return QRCodeWriter.CreateQrCode(code, 500, QRCodeWriter.QrErrorCorrectionLevel.Medium)
            .SetMargins(30)
            .ToPngStream();
    }
}
