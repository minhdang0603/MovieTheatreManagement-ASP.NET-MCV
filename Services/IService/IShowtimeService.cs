using Models;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
	public interface IShowtimeService
	{
		Showtime GetShowtimeById(int id);
		List<Showtime> GetShowtimeList();
		List<Showtime> GetShowtimeListByMovieId(int movieId);
		void AddShowtime(Showtime showtime);
		void RemoveShowtime(Showtime showtime);
		void UpdateShowtime(Showtime showtime);
		IEnumerable<Room> GetAvailableRooms(DateTime startTime, int movieId, int? showtimeId);
		List<int> GetBookedSeats(int showtimeId);
		BatchShowtimeResult CreateBatchShowtimes(BatchShowtimeVM batchVM);
        (List<Showtime> Showtimes, int TotalCount) GetShowtimeListPaginated(string searchTerm, int? movieId, int? roomId, DateTime? startDate, DateTime? endDate, int page, int pageSize);
    }
}
