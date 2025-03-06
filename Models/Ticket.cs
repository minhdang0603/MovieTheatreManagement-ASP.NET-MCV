using System;
using System.Collections.Generic;

namespace Models;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int? SeatId { get; set; }

    public int? BookingId { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual Seat? Seat { get; set; }
}
