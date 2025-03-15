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
	public class DirectorService : IDirectorService
	{
		private readonly ApplicationDbContext _context;

		public DirectorService(ApplicationDbContext context)
		{
			_context = context;
		}

		public void AddDirector(Director director)
		{
			_context.Directors.Add(director);
		}

		public Director GetDirectorById(int id) => _context.Directors.FirstOrDefault(d => d.DirectorId == id);

		public List<Director> GetDirectorList() => _context.Directors.ToList();

		public void RemoveDirector(Director director)
		{
			_context.Directors.Remove(director);
		}

		public void UpdateDirector(Director director)
		{
			var oldDirector = _context.Directors.FirstOrDefault(d => d.DirectorId == director.DirectorId);
			oldDirector.DirectorName = director.DirectorName;
			_context.Directors.Update(oldDirector);
		}
	}
}
