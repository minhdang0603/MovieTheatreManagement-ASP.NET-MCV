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
