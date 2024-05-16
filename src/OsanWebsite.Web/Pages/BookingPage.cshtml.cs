using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OsanWebsite.Web.Pages
{
    public class BookingPageModel : PageModel
    {
        public string? CampaignId { get; set; }

        public void OnGet(string? campaign)
        {
            CampaignId = campaign;
        }
    }
}
