using OsanWebsite.Core.Models;

namespace OsanWebsite.Web.PagesData;

public class SideContentModel
{
    public IEnumerable<Service> Services { get; set; } = default!;
    public Event? NextEvent { get; set; }
}
