using Models;
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
		void AddShowtime(Showtime showtime);
		void RemoveShowtime(Showtime showtime);
		void UpdateShowtime(Showtime showtime);
	}
}
