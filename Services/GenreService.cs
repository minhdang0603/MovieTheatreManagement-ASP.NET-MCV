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
		}

		public void UpdateGenre(Genre genre)
		{
			var oldGenre = _context.Genres.FirstOrDefault(g => g.GenreId == genre.GenreId);
			oldGenre.GenreName = genre.GenreName;
			_context.Genres.Update(oldGenre);
		}
	}
}
