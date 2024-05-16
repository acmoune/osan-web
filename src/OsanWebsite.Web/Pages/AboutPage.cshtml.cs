using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OsanWebsite.Core.Models;
using OsanWebsite.Core.Repositories;

namespace OsanWebsite.Web.Pages
{
    public class AboutPageModel : PageModel
    {
        private IBookingRepository _bookingRepo;
        private IEventsRepository _eventsRepo;

        public IEnumerable<Service> Services { get; set; } = default!;
        public Event? NextEvent { get; set; } = default!;

        public AboutPageModel(IBookingRepository bookingRepo, IEventsRepository eventsRepository)
        {
            _bookingRepo = bookingRepo;
            _eventsRepo = eventsRepository;
        }

        public async Task OnGetAsync()
        {
            Services = await _bookingRepo.GetServicesAsync();
            NextEvent = await _eventsRepo.GetNextEventAsync();
        }
    }
}
