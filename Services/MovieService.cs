using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class MovieService : IMovieService
	{

		private readonly ApplicationDbContext _context;

		public MovieService(ApplicationDbContext context)
		{
			_context = context;
		}

		public List<Movie> GetMovieWithAllRelation()
		{
			var movies = _context.Movies
				.Include(m => m.Director)
				.Include(m => m.Status)
				.Include(m => m.MovieGenres)
					.ThenInclude(mg => mg.Genre)
				.ToList();
			return movies;
		}

		public Movie GetMovieWithAllRelationById(int id)
		{
			var movie = _context.Movies
				.Include(m => m.Director)
				.Include(m => m.Status)
				.Include(m => m.MovieGenres)
					.ThenInclude(mg => mg.Genre)
				.FirstOrDefault(m => m.MovieId == id);
			if (movie == null)
				return null;
			return movie;
		}

		public List<Movie> GetMovieList() => _context.Movies.ToList();

		public Movie GetMovieById(int id) => _context.Movies.FirstOrDefault(m => m.MovieId == id);

		public void AddMovie(Movie movie)
		{
			_context.Movies.Add(movie);
		}

		public void RemoveMovie(Movie movie)
		{
			_context.Movies.Remove(movie);
		}

		public void UpdateMovie(Movie movie)
		{
			var oldMovie = _context.Movies
				.Include(movie => movie.MovieGenres)
				.FirstOrDefault(m => m.MovieId == movie.MovieId);
			if (oldMovie == null)
			{
				throw new Exception("Movie not found");
			}
			oldMovie.Title = movie.Title;
			oldMovie.Description = movie.Description;
			oldMovie.ReleaseDate = movie.ReleaseDate;
			oldMovie.StatusId = movie.StatusId;
			oldMovie.DirectorId = movie.DirectorId;
			oldMovie.Duration = movie.Duration;
			oldMovie.MovieGenres = movie.MovieGenres;
			if(movie.ImageUrl != null)
				oldMovie.ImageUrl = movie.ImageUrl;
			_context.Movies.Update(oldMovie);
		}

	}
}
