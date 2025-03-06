using System;
using System.Collections.Generic;

namespace Models;

public partial class Seat
{
    public int SeatId { get; set; }

    public int? RoomId { get; set; }

    public int? TypeId { get; set; }

    public string SeatRow { get; set; } = null!;

    public int SeatColumn { get; set; }

    public virtual Room? Room { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual SeatType? Type { get; set; }
}
