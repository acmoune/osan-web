using Microsoft.AspNetCore.Mvc.RazorPages;
using OsanWebsite.Core.Models;
using OsanWebsite.Core.Repositories;
using OsanWebsite.Core.Services;

namespace OsanWebsite.Web.Pages
{
    public class EventModel : PageModel
    {
        private IEventsRepository _repo;
        private IBookingService _bookingSvc;
        public Event Event { get; set; } = default!;
        public BookingCampaign Campaign { get; set; } = default!;

        public EventModel(IEventsRepository repo, IBookingService bookingSvc)
        {
            _repo = repo;
            _bookingSvc = bookingSvc;
        }

        public async Task OnGetAsync(string slug)
        {
            Event = await _repo.GetEventAsync(slug);

            if (Event.Archived == false && Event.Date >= DateOnly.FromDateTime(DateTime.Now))
            {
                Campaign = await _bookingSvc.GetOrCreateCampaign(Event.Date, Event.Service);
            }
        }
    }
}
