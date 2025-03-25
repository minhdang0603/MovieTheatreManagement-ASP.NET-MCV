using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using Services.IService;

namespace MovieTheatreManagement.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class MovieController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public MovieController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int statusId)
        {
            if (statusId != 1 && statusId != 2)
            {
                statusId = 1;
            }

            var movies = _unitOfWork.Movie.GetMovieWithAllRelation()
                .Where(m => m.StatusId == statusId)
                .ToList();

            var viewModel = new MovieListVM
            {
                Movies = movies,
                StatusId = statusId,
                CategoryName = statusId == 1 ? "Now Showing" : "Coming Soon",
                Genres = _unitOfWork.Genre.GetGenreList()
            };

            return View(viewModel);
        }

        //public IActionResult All()
        //{
        //    var movies = _unitOfWork.Movie.GetMovieWithAllRelation();

        //    var viewModel = new MovieListVM
        //    {
        //        Movies = movies.ToList(),
        //        CategoryName = "All Movies",
        //        Genres = _unitOfWork.Genre.GetGenreList()
        //    };

        //    return View("Index", viewModel);
        //}

        //public IActionResult ByGenre(int genreId)
        //{
        //    var genre = _unitOfWork.Genre.GetGenreById(genreId);
        //    if (genre == null)
        //    {
        //        return NotFound();
        //    }

        //    var movies = _unitOfWork.Movie.GetMovieWithAllRelation()
        //        .Where(m => m.MovieGenres.Any(mg => mg.GenreId == genreId))
        //        .ToList();

        //    var viewModel = new MovieListVM
        //    {
        //        Movies = movies,
        //        CategoryName = $"{genre.GenreName} Movies",
        //        GenreId = genreId,
        //        Genres = _unitOfWork.Genre.GetGenreList()
        //    };

        //    return View("Index", viewModel);
        //}

        //public IActionResult Search(string query)
        //{
        //    if (string.IsNullOrWhiteSpace(query))
        //    {
        //        return RedirectToAction("All");
        //    }

        //    query = query.Trim().ToLower();

        //    var movies = _unitOfWork.Movie.GetMovieWithAllRelation()
        //        .Where(m =>
        //            m.Title.ToLower().Contains(query) ||
        //            (m.Director != null && m.Director.DirectorName.ToLower().Contains(query)) ||
        //            m.MovieGenres.Any(mg => mg.Genre.GenreName.ToLower().Contains(query)))
        //        .ToList();

        //    var viewModel = new MovieListVM
        //    {
        //        Movies = movies,
        //        CategoryName = $"Search Results for \"{query}\"",
        //        SearchQuery = query,
        //        Genres = _unitOfWork.Genre.GetGenreList()
        //    };

        //    return View("Index", viewModel);
        //}
    }
}
