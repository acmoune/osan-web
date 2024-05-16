using OsanWebsite.Core.Models;

namespace OsanWebsite.Core.Repositories;

public interface IEventsRepository
{
    Task<PaggingResult<Event>> GetEventsAsync(bool archived, int page, int pageSize);
    Task<Event> GetEventAsync(string slug);
    Task<Event?> GetNextEventAsync();
}
