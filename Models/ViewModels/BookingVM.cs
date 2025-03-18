using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
	public class BookingVM
	{
		public Showtime Showtime { get; set; }
		public Movie Movie { get; set; }
		public Room Room { get; set; }

		public List<SeatType> SeatTypes { get; set; }

		[ValidateNever]
		public List<Seat> AllSeats { get; set; } = new List<Seat>();

		[ValidateNever]
		public List<int> BookedSeats { get; set; } = new List<int>();

		// Helper property to check if a seat is booked
		public bool IsSeatBooked(int seatId)
		{
			return BookedSeats.Contains(seatId);
		}

		// Helper method to get seats grouped by row
		public IEnumerable<IGrouping<string, Seat>> SeatsByRow
		{
			get
			{
				return AllSeats.GroupBy(s => s.SeatRow).OrderBy(g => g.Key);
			}
		}
	}
}
