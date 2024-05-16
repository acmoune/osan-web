using Microsoft.AspNetCore.Mvc;
using OsanWebsite.Core.Repositories;
using OsanWebsite.Web.PagesData;

namespace OsanWebsite.Web.ViewComponents;

public class SideContent : ViewComponent
{
    private SideContentModel _sideContent = new SideContentModel();
    private IBookingRepository _bookingRepo;
    private IEventsRepository _eventsRepo;

    public SideContent(IBookingRepository bookingRepo, IEventsRepository eventsRepo)
    {
        _bookingRepo = bookingRepo;
        _eventsRepo = eventsRepo;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        _sideContent.Services = await _bookingRepo.GetServicesAsync();
        _sideContent.NextEvent = await _eventsRepo.GetNextEventAsync();

        return View(_sideContent);
    }
}
