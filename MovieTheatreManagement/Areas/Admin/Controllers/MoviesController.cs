using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.ViewModels;
using Services.IService;
using Utility;

namespace MovieTheatreManagement.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin + ", " + SD.Role_Employee)]
	public class MoviesController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _hostEnvironment;

		public MoviesController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_hostEnvironment = hostEnvironment;
		}

		public IActionResult Index()
		{
			var movies = _unitOfWork.Movie.GetMovieWithAllRelation();
			return View(movies);
		}

		public IActionResult Upsert(int? id)
		{
			var movieVM = new MovieVM()
			{
				Movie = new Movie(),
				GenreList = _unitOfWork.Genre.GetGenreList().Select(g => new SelectListItem()
				{
					Text = g.GenreName,
					Value = g.GenreId.ToString()
				}),
				DirectorList = _unitOfWork.Director.GetDirectorList().Select(d => new SelectListItem()
				{
					Text = d.DirectorName,
					Value = d.DirectorId.ToString()
				}),
				StatusList = _unitOfWork.MovieStatus.GetMovieStatusList().Select(ms => new SelectListItem()
				{
					Text = ms.StatusName,
					Value = ms.StatusId.ToString()
				})
			};

			if (id is null or 0)
			{
				return View(movieVM);
			}

			movieVM.Movie = _unitOfWork.Movie.GetMovieWithAllRelationById(id.Value);

			if (movieVM.Movie == null)
				return NotFound();

			return View(movieVM);

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(int id, MovieVM movieVM, IFormFile? file, string[] selectedGenres)
		{
			if (id != movieVM.Movie.MovieId)
			{
				return NotFound();
			}

			List<MovieGenre> genreList = new List<MovieGenre>();

			if (selectedGenres != null && selectedGenres.Length > 0)
			{
				foreach (var genreId in selectedGenres)
				{
					genreList.Add(new MovieGenre
					{
						GenreId = int.Parse(genreId),
						MovieId = movieVM.Movie.MovieId
					});
				}
			}

			if (!ModelState.IsValid)
			{
				if (movieVM.Movie.MovieId != 0)
				{
					movieVM.Movie = _unitOfWork.Movie.GetMovieWithAllRelationById(movieVM.Movie.MovieId);
					movieVM.Movie.MovieGenres = genreList;
				}
				movieVM.StatusList = _unitOfWork.MovieStatus.GetMovieStatusList().Select(ms => new SelectListItem()
				{
					Text = ms.StatusName,
					Value = ms.StatusId.ToString()
				});
				movieVM.GenreList = _unitOfWork.Genre.GetGenreList().Select(g => new SelectListItem()
				{
					Text = g.GenreName,
					Value = g.GenreId.ToString()
				});
				movieVM.DirectorList = _unitOfWork.Director.GetDirectorList().Select(d => new SelectListItem()
				{
					Text = d.DirectorName,
					Value = d.DirectorId.ToString()
				});
				return View(movieVM);
			}

			movieVM.Movie.MovieGenres = genreList;

			string webRootPath = _hostEnvironment.WebRootPath;
			if (file is not null)
			{
				string fileName = IOUtils.UploadImage(webRootPath, file, movieVM.Movie.ImageUrl);

				movieVM.Movie.ImageUrl = fileName;
			}

			try
			{
				if (id == 0)
				{
					_unitOfWork.Movie.AddMovie(movieVM.Movie);
					TempData["success"] = "Movie added successfully";
				}
				else
				{
					_unitOfWork.Movie.UpdateMovie(movieVM.Movie);
					TempData["success"] = "Movie updated successfully";
				}
				_unitOfWork.Save();
			}
			catch (Exception)
			{
				TempData["error"] = "There are some error when performing action!";
				throw;
			}
			return RedirectToAction(nameof(Index));
		}

		#region API CALLS
		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			if (id is null or 0)
			{
				return NotFound();
			}

			var movie = _unitOfWork.Movie.GetMovieById(id.Value);

			if (movie is null)
			{
				return NotFound();
			}

			if (!string.IsNullOrEmpty(movie.ImageUrl))
			{
				// Delete the existing file
				var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, movie.ImageUrl.TrimStart('\\'));
				if (System.IO.File.Exists(oldImagePath))
				{
					System.IO.File.Delete(oldImagePath);
				}
			}

			_unitOfWork.Movie.RemoveMovie(movie);
			_unitOfWork.Save();
			TempData["success"] = "Delete successful";
			return Json(new { });
		}
		#endregion
	}
}
