using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
	public class BookingConfirmationVM
	{
		public Showtime Showtime { get; set; }
		public Movie Movie { get; set; }

		[ValidateNever]
		public List<Seat> SelectedSeats { get; set; } = new List<Seat>();

		public decimal TotalPrice { get; set; }

		// Format selected seats for display (e.g., "A1, A2, B3")
		public string FormattedSelectedSeats
		{
			get
			{
				return string.Join(", ", SelectedSeats.Select(s => $"{s.SeatRow}{s.SeatColumn + 1}"));
			}
		}
	}
}
