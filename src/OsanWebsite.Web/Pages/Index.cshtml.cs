using Microsoft.AspNetCore.Mvc.RazorPages;
using OsanWebsite.Core.Models;
using OsanWebsite.Core.Repositories;

namespace OsanWebsite.Web.Pages;

public class IndexModel : PageModel
{
    public string Feed { get; set; } = default!;
    public Event? NextEvent { get; set; }

    private IPicturesRepo _picturesRepo;
    private IEventsRepository _eventsRepository;

    public PictureSet SlideShow { get; set; } = default!;
    public PictureSet Galery { get; set; } = default!;

    public IndexModel(IPicturesRepo picturesRepo, IEventsRepository eventsRepository)
    {
        _picturesRepo = picturesRepo;
        _eventsRepository = eventsRepository;
    }

    public async Task OnGetAsync(string? feed)
    {
        Feed = feed ?? "spotlights";
        SlideShow = await _picturesRepo.GetSlides();
        NextEvent = await _eventsRepository.GetNextEventAsync();
    }
}
