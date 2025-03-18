using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using MovieTheatreManagement.Models;
using Services.IService;

namespace MovieTheatreManagement.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class HomeController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			var movies = _unitOfWork.Movie.GetMovieWithAllRelation();
			return View(movies);
		}

		public IActionResult Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var movie = _unitOfWork.Movie.GetMovieWithAllRelationById(id.Value);
			if (movie == null)
			{
				return NotFound();
			}

			var showtimes = _unitOfWork.Showtime.GetShowtimeListByMovieId(id.Value);

			var viewModel = new MovieDetailsVM
			{
				Movie = movie,
				Showtimes = showtimes
			};

			return View(viewModel);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
