using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
	public interface IUnitOfWork
	{
		IGenreService Genre { get; }
		IDirectorService Director { get; }
		IMovieService Movie { get; }
		IMovieStatusService MovieStatus { get; }
		IRoomService Room { get; }
		ISeatTypeService SeatType { get; }
		IShowtimeService Showtime { get; }

		void Save();
	}
}
