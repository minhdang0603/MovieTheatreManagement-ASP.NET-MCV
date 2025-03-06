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
	public class GenreService : IGenreService
	{
		private readonly ApplicationDbContext _context;

		public GenreService(ApplicationDbContext context)
		{
			_context = context;
		}

		public void AddGenre(Genre genre)
		{
			_context.Genres.Add(genre);
			_context.SaveChanges();
		}

		public Genre GetGenreById(int id)
		{
			return _context.Genres.FirstOrDefault(g => g.GenreId == id);
		}

		public List<Genre> GetGenreList()
		{
			return _context.Genres.ToList();
		}

		public void RemoveGenre(Genre genre)
		{
			_context.Genres.Remove(genre);
			_context.SaveChanges();
		}

		public bool UpdateGenre(Genre genre)
		{
			var oldGenre = _context.Genres.FirstOrDefault(g => g.GenreId == genre.GenreId);
			if(oldGenre == null)
				return false;
			oldGenre.GenreName = genre.GenreName;
			_context.Genres.Update(oldGenre);
			_context.SaveChanges();
			return true;
		}
	}
}
