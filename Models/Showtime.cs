using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace Models;

public partial class Showtime
{
    public int ShowtimeId { get; set; }

    public int MovieId { get; set; }

    public int RoomId { get; set; }

    public DateTime StartTime { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [ValidateNever]
    public virtual Movie Movie { get; set; }

	[ValidateNever]
	public virtual Room Room { get; set; }
}
