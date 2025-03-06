using System;
using System.Collections.Generic;

namespace Models;

public partial class SeatType
{
    public int TypeId { get; set; }

    public string? TypeName { get; set; }

    public int? Price { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
