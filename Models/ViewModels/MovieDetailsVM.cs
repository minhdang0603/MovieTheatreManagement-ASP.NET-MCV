using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
	public class MovieDetailsVM
	{
		public Movie Movie { get; set; }

		[ValidateNever]
		public List<Showtime> Showtimes { get; set; } = new List<Showtime>();

		// Helper method to get showtimes grouped by date
		public IEnumerable<IGrouping<DateOnly, Showtime>> ShowtimesByDate
		{
			get
			{
				return Showtimes
					.GroupBy(s => DateOnly.FromDateTime(s.StartTime))
					.OrderBy(g => g.Key);
			}
		}

		// Helper method to get showtimes grouped by room
		public IEnumerable<IGrouping<int, Showtime>> GetShowtimesByRoom(DateOnly date)
		{
			return Showtimes
				.Where(s => DateOnly.FromDateTime(s.StartTime) == date)
				.GroupBy(s => s.RoomId)
				.OrderBy(g => g.Key);
		}
	}
}
