﻿using MediatR;
using OsanWebsite.Core.Models;

namespace OsanWebsite.Core.Events;

public class BookingConfirmedEvent : INotification
{
    public Booking Booking { get; set; } = default!;
}
