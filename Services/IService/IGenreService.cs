using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
	public interface IGenreService
	{
		Genre GetGenreById(int id);
		List<Genre> GetGenreList();
		void AddGenre(Genre genre);
		void RemoveGenre(Genre genre);
		void UpdateGenre(Genre genre);
	}
}
