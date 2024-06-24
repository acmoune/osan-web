using OsanWebsite.Core.Dtos;
using OsanWebsite.Core.Models;

namespace OsanWebsite.Core.Repositories;

public interface IBookingRepository
{
    Task<Booking> Create(Booking booking);
    Task<Booking> GetById(Guid bookingId);
    Task<Booking> Save(Booking booking);
    Task<PaggingResult<BookingItem>> GetBookings(string campaign_id, int page, int size);

    Task<Service> GetServiceByBame(string name);
    Task<TableType> GetTableTypeByBame(string name);
    Task<IEnumerable<Service>> GetServicesAsync();
    Task<IEnumerable<TableType>> GetTableTypesAsync();

    Task<BookingCampaign?> GetCampaign(DateOnly date, string serviceName);
    Task<BookingCampaign?> GetCampaign(string campaign_id);    
    Task<IEnumerable<BookingCampaign>> GetCampaigns(int year, int month);
    Task<BookingCampaign> SaveCampaign(BookingCampaign campaign);
    Task CompleteCampaign(BookingCampaign campaign);
}
