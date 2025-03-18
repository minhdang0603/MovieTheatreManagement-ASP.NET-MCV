using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;
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
			return _context.Showtimes
				.Include(s => s.Room)
				.Where(s => s.MovieId == movieId && s.StartTime > DateTime.Now)
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
	}
}
