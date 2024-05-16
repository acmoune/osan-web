using OsanWebsite.Core.Dtos;
using OsanWebsite.Core.Models;

namespace OsanWebsite.Core.Services;

public interface IBookingService
{
    Task<Booking> CreateBooking(BookingDto bookingDto);
    Task<Booking> UpdateBooking(Guid bookingId, string name, string email, string phone, string tableType);
    Task<Booking> CancelBooking(Guid bookingId, string reason);
    Task<Booking> ConfirmBooking(Guid bookingId);
    Task<Booking> MarkAsConsumed(Guid bookingId);

    Task<BookingCampaign> GetOrCreateCampaign(DateOnly date, Service service);
    Task<BookingCampaign> CloseCampaign(string campaign_id);
    Task<BookingCampaign> CompleteCampaign(string campaign_id);
}
