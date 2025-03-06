using System;
using System.Collections.Generic;

namespace Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? ShowtimeId { get; set; }

    public int? TotalPrice { get; set; }

    public string? Status { get; set; }

    public virtual Showtime? Showtime { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
