using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.ViewModels;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class ShowtimeService : IShowtimeService
	{
		private readonly ApplicationDbContext _context;

		public ShowtimeService(ApplicationDbContext context)
		{
			_context = context;
		}

		public void AddShowtime(Showtime showtime)
		{
			_context.Showtimes.Add(showtime);
		}

		public IEnumerable<Room> GetAvailableRooms(DateTime startTime, int movieId, int? showtimeId)
		{
			var movie = _context.Movies.FirstOrDefault(m => m.MovieId == movieId);
			if (movie == null)
				throw new Exception("Movie not found");

			DateTime endTime = startTime.AddMinutes(movie.Duration);

			var allRooms = _context.Rooms.ToList();

			var existingShowtimes = _context.Showtimes
				.Include(s => s.Movie)
				.Where(s => s.ShowtimeId != (showtimeId ?? 0))
				.ToList();

			var availableRooms = allRooms.Where(room =>
			{
				foreach (var showtime in existingShowtimes.Where(s => s.RoomId == room.RoomId))
				{
					if (showtime.Movie == null) continue;

					var existingEndTime = showtime.StartTime.AddMinutes(showtime.Movie.Duration);
					var bufferStartTime = showtime.StartTime.AddMinutes(-10);
					var bufferEndTime = existingEndTime.AddMinutes(10);

					if ((startTime >= bufferStartTime && startTime <= bufferEndTime) ||
						(endTime >= bufferStartTime && endTime <= bufferEndTime) ||
						(startTime <= bufferStartTime && endTime >= bufferEndTime))
					{
						// This room has a conflict
						return false;
					}
				}
				return true;
			});

			return availableRooms;
		}

		public List<int> GetBookedSeats(int showtimeId)
		{
			return _context.Bookings
				.Where(b => b.ShowtimeId == showtimeId && b.Status != "cancelled")
				.SelectMany(b => b.Tickets)
				.Select(t => t.SeatId ?? 0)
				.Where(id => id != 0)
				.ToList();
		}

		public Showtime GetShowtimeById(int id)
		{
			return _context.Showtimes
				.Include(s => s.Movie)
				.Include(s => s.Room)
				.FirstOrDefault(s => s.ShowtimeId == id);
		}

		public List<Showtime> GetShowtimeList()
		{
			return _context.Showtimes
				.Include(s => s.Movie)
				.Include(s => s.Room)
				.Where(s => s.StartTime >= DateTime.Now)
				.OrderBy(s => s.StartTime)
				.ToList();
		}

        public List<Showtime> GetShowtimeListByMovieId(int movieId)
        {
            DateTime fiveDaysFromNow = DateTime.Now.AddDays(5);
            return _context.Showtimes
                .Include(s => s.Room)
                .Where(s => s.MovieId == movieId && s.StartTime > DateTime.Now && s.StartTime <= fiveDaysFromNow)
                .OrderBy(s => s.StartTime)
                .ToList();
        }

		public void RemoveShowtime(Showtime showtime)
		{
			_context.Showtimes.Remove(showtime);
		}

		public void UpdateShowtime(Showtime showtime)
		{
			var oldShowtime = _context.Showtimes.FirstOrDefault(s => s.ShowtimeId == showtime.ShowtimeId);
			if (oldShowtime == null)
				throw new Exception("Showtime not found");

			oldShowtime.MovieId = showtime.MovieId;
			oldShowtime.RoomId = showtime.RoomId;
			oldShowtime.StartTime = showtime.StartTime;

			_context.Showtimes.Update(oldShowtime);
		}

		public BatchShowtimeResult CreateBatchShowtimes(BatchShowtimeVM batchVM)
		{
			var result = new BatchShowtimeResult();
			var movie = _context.Movies.Find(batchVM.MovieId);

			if (movie == null)
			{
				result.ErrorMessages.Add("Movie not found.");
				return result;
			}

			// Validate dates
			if (batchVM.StartDate > batchVM.EndDate)
			{
				result.ErrorMessages.Add("Start date must be before end date.");
				return result;
			}

			if (batchVM.SelectedDays.Count == 0)
			{
				result.ErrorMessages.Add("No days selected.");
				return result;
			}

			if (batchVM.TimeSlots.Count == 0)
			{
				result.ErrorMessages.Add("No time slots selected.");
				return result;
			}

			if (batchVM.PreferredRoomIds.Count == 0)
			{
				result.ErrorMessages.Add("No rooms selected.");
				return result;
			}

			// Generate all possible showtimes based on the criteria
			var possibleShowtimes = new List<DateTime>();
			for (var date = batchVM.StartDate.Date; date <= batchVM.EndDate.Date; date = date.AddDays(1))
			{
				if (batchVM.SelectedDays.Contains(date.DayOfWeek))
				{
					foreach (var timeSlot in batchVM.TimeSlots)
					{
						possibleShowtimes.Add(date.Add(timeSlot));
					}
				}
			}

			if (possibleShowtimes.Count == 0)
			{
				result.ErrorMessages.Add("No possible show time on the selected day.");
				return result;
			}

			// Filter out past dates
			possibleShowtimes = possibleShowtimes.Where(s => s > DateTime.Now).ToList();

			// Create showtimes
			foreach (var startTime in possibleShowtimes)
			{
				bool showtimeCreated = false;

				// Try each preferred room until one works
				foreach (var roomId in batchVM.PreferredRoomIds)
				{
					// Only try other rooms if auto-resolve is enabled or this is the first preferred room
					if (!batchVM.AutoResolveConflicts && roomId != batchVM.PreferredRoomIds.First())
					{
						break;
					}

					try
					{
						var availableRooms = GetAvailableRooms(startTime, batchVM.MovieId, null);
						if (availableRooms.Any(r => r.RoomId == roomId))
						{
							// Create the showtime
							var showtime = new Showtime
							{
								MovieId = batchVM.MovieId,
								RoomId = roomId,
								StartTime = startTime
							};

							_context.Showtimes.Add(showtime);
							_context.SaveChanges();

							// Load related entities for display
							_context.Entry(showtime).Reference(s => s.Movie).Load();
							_context.Entry(showtime).Reference(s => s.Room).Load();

							result.CreatedShowtimes.Add(showtime);
							showtimeCreated = true;
							break;
						}
					}
					catch (Exception ex)
					{
						// Continue to the next room
					}
				}

				if (!showtimeCreated)
				{
					var roomNames = string.Join(", ", _context.Rooms
						.Where(r => batchVM.PreferredRoomIds.Contains(r.RoomId))
						.Select(r => r.Name));

					result.ErrorMessages.Add($"Could not create showtime for {startTime:yyyy-MM-dd HH:mm}. No available rooms among: {roomNames}");
				}
			}

			return result;
		}


	}
}
