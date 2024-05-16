using OsanWebsite.Core.Models;

namespace OsanWebsite.Core.Repositories;

public interface IPicturesRepo
{
    Task<PictureSet> GetSlides();
}
