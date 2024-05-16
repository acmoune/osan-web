using Microsoft.AspNetCore.Mvc.RazorPages;
using OsanWebsite.Core.Models;
using OsanWebsite.Core.Repositories;

namespace OsanWebsite.Web.Pages
{
    public class SpotModel : PageModel
    {
        private ISpotsRepository _repo;

        public Spot Spot { get; set; } = default!;

        public SpotModel(ISpotsRepository repo) => _repo = repo;

        public async Task OnGetAsync(string slug)
        {
            Spot = await _repo.GetSpotAsync(slug);
        }
    }
}
