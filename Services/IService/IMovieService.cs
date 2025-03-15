using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
	public interface IMovieService
	{
		void AddMovie(Movie movie);
		Movie GetMovieWithAllRelationById(int id);
		List<Movie> GetMovieWithAllRelation();
		void RemoveMovie(Movie movie);
		void UpdateMovie(Movie movie);
		Movie GetMovieById(int id);
		List<Movie> GetMovieList();
	}
}
