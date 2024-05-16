using OsanWebsite.Core.Models;

namespace OsanWebsite.Core.Repositories;
public interface ISpotsRepository
{
    Task<PaggingResult<Spot>> GetSpotsAsync(int page, int pageSize);
    Task<Spot> GetSpotAsync(string slug);
}
