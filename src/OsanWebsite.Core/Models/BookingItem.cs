namespace OsanWebsite.Core.Models;

public class BookingItem
{
    public Booking Booking { get; set; } = default!;
    public bool Confirming { get; set; } = false;
    public bool Cancelling { get; set; } = false;
    public bool Consuming { get; set; } = false;

    public BookingItem(Booking booking)
    {
        this.Booking = booking;
    }
}
