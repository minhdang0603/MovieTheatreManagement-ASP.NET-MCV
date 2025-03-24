using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
	public class BatchShowtimeResult
	{
		public List<Showtime> CreatedShowtimes { get; set; } = new List<Showtime>();
		public List<string> ErrorMessages { get; set; } = new List<string>();
	}
}
