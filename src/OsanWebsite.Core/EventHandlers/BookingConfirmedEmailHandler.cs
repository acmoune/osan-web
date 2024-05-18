using MediatR;
using OsanWebsite.Core.Events;
using OsanWebsite.Core.Infrastructure;

namespace OsanWebsite.Core.EventHandlers;

public class BookingConfirmedEmailHandler : INotificationHandler<BookingConfirmedEvent>
{
    private readonly IEmailService _emailSender;

    public BookingConfirmedEmailHandler(IEmailService emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task Handle(BookingConfirmedEvent notification, CancellationToken cancellationToken)
    {
        var to = notification.Booking.CustomerEmail;
        var subject = $"Confirmation de votre réservation ({notification.Booking.Campaign.Name()})";
        var body = $@"Salut {notification.Booking.CustomerName},

Nous confirmons votre réservation du {notification.Booking.Campaign.BookingDate}, pour le service {notification.Booking.Campaign.Service.Name}.
Ce service va de {notification.Booking.Campaign.Service.StartTime} à {notification.Booking.Campaign.Service.EndTime}.

Veuillez télécharger votre PASS (QR Code) en pièce jointe.
Vous devrez le présenter à l'entrée pour être identifié. Veuillez également le partager à tous ceux qui seront à votre table.

Nous vous disons à bientôt.

Cordialement,
OSAN Cave.
";
        await _emailSender.SendMailWithPng(to, subject, body, notification.Booking.QrCodeUrl!);
    }
}
