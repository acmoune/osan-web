using MediatR;
using OsanWebsite.Core.Events;
using OsanWebsite.Core.Infrastructure;

namespace OsanWebsite.Core.EventHandlers;

public class BookingCancelledEmailHandler : INotificationHandler<BookingCancelledEvent>
{
    private readonly IEmailService _emailSender;

    public BookingCancelledEmailHandler(IEmailService emailSender) => _emailSender = emailSender;

    public async Task Handle(BookingCancelledEvent notification, CancellationToken cancellationToken)
    {
        var to = notification.Booking.CustomerEmail;
        var subject = $"Annulation de votre réservation ({notification.Booking.Campaign.Name()})";
        var body = $@"Salut {notification.Booking.CustomerName},

Votre réservation du {notification.Booking.Campaign.BookingDate}, pour le service {notification.Booking.Campaign.Service.Name} à bien été annulée.

Nous espérons vous compter bientôt parmi nous.

Cordialement,
OSAN Cave.
";
        await _emailSender.SendMail(to, subject, body);
    }
}
