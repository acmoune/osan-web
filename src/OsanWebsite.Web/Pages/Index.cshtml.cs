using Microsoft.AspNetCore.Mvc.RazorPages;
using OsanWebsite.Core.Models;
using OsanWebsite.Core.Repositories;

namespace OsanWebsite.Web.Pages;

public class IndexModel : PageModel
{
    public string Feed { get; set; } = default!;
    private IPicturesRepo _picturesRepo;

    public PictureSet SlideShow { get; set; } = default!;
    public PictureSet Galery { get; set; } = default!;

    public IndexModel(IPicturesRepo picturesRepo)
    {
        _picturesRepo = picturesRepo;
    }

    public async Task OnGetAsync(string? feed)
    {
        Feed = feed ?? "calendar";
        SlideShow = await _picturesRepo.GetSlides();
    }
}
