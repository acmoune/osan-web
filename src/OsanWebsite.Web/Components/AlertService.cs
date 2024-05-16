using Blazorise;

namespace OsanWebsite.Web.Components;

public class AlertService
{
    private IToastService _toastService;

    public AlertService(IToastService toastService)
    {
        _toastService = toastService;
    }

    public Task Info(string message)
    {
        return _toastService.Info(message, "Info");
    }

    public Task Error(string message)
    {
        return _toastService.Error(message, "Error");
    }
}
