using System;
using System.Collections.Generic;

namespace Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string Name { get; set; } = null!;

    public int TotalColumns { get; set; }
    public int TotalRows { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
