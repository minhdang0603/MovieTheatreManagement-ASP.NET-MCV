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
	public class SeatTypeService : ISeatTypeService
	{
		private readonly ApplicationDbContext _context;

		public SeatTypeService(ApplicationDbContext context)
		{
			_context = context;
		}

		public List<SeatType> GetSeatTypeList()
		{
			return _context.SeatTypes.ToList();
		}
	}
}
