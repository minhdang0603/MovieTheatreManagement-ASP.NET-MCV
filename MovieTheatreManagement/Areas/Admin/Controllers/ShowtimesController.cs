using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.ViewModels;
using Services.IService;
using Utility;

namespace MovieTheatreManagement.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin + ", " + SD.Role_Employee)]
	public class ShowtimesController : Controller
	{

		private readonly IUnitOfWork _unitOfWork;

		public ShowtimesController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			var showtimes = _unitOfWork.Showtime.GetShowtimeList();
			return View(showtimes);
		}

		public IActionResult Upsert(int? id)
		{
			var showtimeVM = new ShowtimeVM
			{
				Showtime = new Showtime(),
				MovieList = _unitOfWork.Movie.GetMovieList().Select(m => new SelectListItem()
				{
					Text = m.Title,
					Value = m.MovieId.ToString()
				}),
				RoomList = _unitOfWork.Room.GetRoomList().Select(r => new SelectListItem()
				{
					Text = r.Name,
					Value = r.RoomId.ToString()
				})
			};

			if (id is null or 0)
			{
				return View(showtimeVM);
			}

			showtimeVM.Showtime = _unitOfWork.Showtime.GetShowtimeById(id.Value);

			if (showtimeVM.Showtime is null)
			{
				return NotFound();
			}

			return View(showtimeVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(int id, ShowtimeVM showtimeVM)
		{
			if (id != showtimeVM.Showtime.ShowtimeId)
			{
				return NotFound();
			}

			if (!ModelState.IsValid)
			{
				showtimeVM.MovieList = _unitOfWork.Movie.GetMovieList().Select(m => new SelectListItem()
				{
					Text = m.Title,
					Value = m.MovieId.ToString()
				});
				showtimeVM.RoomList = _unitOfWork.Room.GetRoomList().Select(r => new SelectListItem()
				{
					Text = r.Name,
					Value = r.RoomId.ToString()
				});
				return View(showtimeVM);
			}

			try
			{
				if (showtimeVM.Showtime.ShowtimeId == 0)
				{
					_unitOfWork.Showtime.AddShowtime(showtimeVM.Showtime);
					TempData["success"] = "Showtime added successfully";
				}
				else
				{
					_unitOfWork.Showtime.UpdateShowtime(showtimeVM.Showtime);
					TempData["success"] = "Showtime updated successfully";
				}
				_unitOfWork.Save();
			}
			catch (Exception)
			{
				TempData["error"] = "An error occurred while performing action";
			}
			return RedirectToAction(nameof(Index));
		}

		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			if (id is null or 0)
			{
				return NotFound();
			}
			var showtime = _unitOfWork.Showtime.GetShowtimeById(id.Value);
			if (showtime is null)
			{
				return NotFound();
			}
			_unitOfWork.Showtime.RemoveShowtime(showtime);
			_unitOfWork.Save();
			TempData["success"] = "Delete successful";
			return Json(new { });
		}
	}
}
