using MediatR;
using OsanWebsite.Core.Models;

namespace OsanWebsite.Core.Events;

public class BookingCreatedEvent : INotification
{
    public Booking Booking { get; set; } = default!;
}
