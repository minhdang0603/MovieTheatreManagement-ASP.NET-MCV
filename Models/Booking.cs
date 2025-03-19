using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? ShowtimeId { get; set; }

    public int? TotalPrice { get; set; }

    public string? Status { get; set; }

	public int PaymentId { get; set; }
    public virtual Payment Payment { get; set; }

	public string ApplicationUserId { get; set; }

    [ForeignKey(nameof(ApplicationUserId))]
    [ValidateNever]
	public virtual ApplicationUser ApplicationUser { get; set; }

	public virtual Showtime? Showtime { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
