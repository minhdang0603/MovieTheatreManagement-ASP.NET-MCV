using DataAccess.Data;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;

		public IGenreService Genre { get; private set; }
		public IDirectorService Director { get; private set; }
		public IMovieService Movie { get; private set; }
		public IMovieStatusService MovieStatus { get; private set; }
		public IRoomService Room { get; private set; }
		public ISeatTypeService SeatType { get; private set; }
		public IShowtimeService Showtime { get; private set; }
		public IBookingService Booking { get; private set; }

		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
			Genre = new GenreService(_context);
			Director = new DirectorService(_context);
			Movie = new MovieService(_context);
			MovieStatus = new MovieStatusService(_context);
			Room = new RoomService(_context);
			SeatType = new SeatTypeService(_context);
			Showtime = new ShowtimeService(_context);
			Booking = new BookingService(_context);
		}

		public void Save()
		{
			_context.SaveChanges();
		}
	}
}
