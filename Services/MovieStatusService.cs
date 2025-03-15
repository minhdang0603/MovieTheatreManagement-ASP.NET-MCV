using DataAccess.Data;
using Models;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class MovieStatusService : IMovieStatusService
	{
		private readonly ApplicationDbContext _context;

		public MovieStatusService(ApplicationDbContext context)
		{
			_context = context;
		}

		public List<MovieStatus> GetMovieStatusList()
		{
			return _context.MovieStatuses.ToList();
		}
	}
}
