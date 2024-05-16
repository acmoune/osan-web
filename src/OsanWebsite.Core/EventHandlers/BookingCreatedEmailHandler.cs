using MediatR;
using OsanWebsite.Core.Events;
using OsanWebsite.Core.Infrastructure;

namespace OsanWebsite.Core.EventHandlers;

public class BookingCreatedEmailHandler : INotificationHandler<BookingCreatedEvent>
{
    private readonly IEmailService _emailSender;

    public BookingCreatedEmailHandler(IEmailService emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task Handle(BookingCreatedEvent notification, CancellationToken cancellationToken)
    {
        var to = Environment.GetEnvironmentVariable("OSAN_MESSAGES_To")!;
        var subject = $"Nouvelle réservation pour ({notification.Booking.Campaign.Name()})";
        var body = @$"Nouvelle réservation depuis le site.

Date: {notification.Booking.Campaign.BookingDate},
Service: {notification.Booking.Campaign.Service.Name},

Client: {notification.Booking.CustomerName},
Téléphone: {notification.Booking.CustomerPhone},
Email: {notification.Booking.CustomerEmail}.

Bien vouloir prendre contact pour confirmation.

Cordialement,
OSAN Cave.
";
        await _emailSender.SendMail(to, subject, body);
    }
}
