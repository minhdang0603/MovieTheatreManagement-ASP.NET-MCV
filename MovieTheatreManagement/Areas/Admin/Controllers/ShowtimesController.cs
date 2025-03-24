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

			if (showtimeVM.Showtime.StartTime < DateTime.Now)
			{
				ModelState.AddModelError("Showtime.StartTime", "You cannot schedule a showtime in the past.");
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

		public IActionResult BatchCreate()
		{
			var batchVM = new BatchShowtimeVM
			{
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

			return View(batchVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult BatchCreate(BatchShowtimeVM batchVM, string[] selectedDays, string[] selectedRooms, string[] timeSlots)
		{
			if (!ModelState.IsValid)
			{
				batchVM.MovieList = _unitOfWork.Movie.GetMovieList().Select(m => new SelectListItem()
				{
					Text = m.Title,
					Value = m.MovieId.ToString()
				});
				batchVM.RoomList = _unitOfWork.Room.GetRoomList().Select(r => new SelectListItem()
				{
					Text = r.Name,
					Value = r.RoomId.ToString()
				});
				return View(batchVM);
			}

			// Process the selected days
			batchVM.SelectedDays = new List<DayOfWeek>();
			foreach (var day in selectedDays)
			{
				if (Enum.TryParse<DayOfWeek>(day, out var dayOfWeek))
				{
					batchVM.SelectedDays.Add(dayOfWeek);
				}
			}

			// Process the selected rooms
			batchVM.PreferredRoomIds = selectedRooms.Select(int.Parse).ToList();

			// Process the time slots
			batchVM.TimeSlots = new List<TimeSpan>();
			foreach (var slot in timeSlots)
			{
				if (TimeSpan.TryParse(slot, out var timeSpan))
				{
					batchVM.TimeSlots.Add(timeSpan);
				}
			}

			try
			{
				// Generate showtimes
				var result = _unitOfWork.Showtime.CreateBatchShowtimes(batchVM);

				// Save the results for the results page
				batchVM.SuccessCount = result.CreatedShowtimes.Count;
				batchVM.FailureCount = result.ErrorMessages.Count;
				batchVM.ErrorMessages = result.ErrorMessages;
				batchVM.CreatedShowtimes = result.CreatedShowtimes;

				if (result.CreatedShowtimes.Count > 0)
				{
					TempData["success"] = $"Successfully created {result.CreatedShowtimes.Count} showtimes.";
				}

				if (result.ErrorMessages.Count > 0)
				{
					TempData["error"] = $"Failed to create {result.ErrorMessages.Count} showtimes. See details for more information.";
				}
			}
			catch (Exception ex)
			{
				batchVM.ErrorMessages.Add($"An error occurred: {ex.Message}");
				TempData["error"] = "An error occurred during batch creation.";
			}

			return View("BatchCreateResults", batchVM);
		}

		#region API CALLS
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

		[HttpGet()]
		public IActionResult AvailableRoom(DateTime startTime, int movieId, int? showtimeId = null)
		{
			try
			{
				// Use the service to get available rooms
				var availableRooms = _unitOfWork.Showtime.GetAvailableRooms(startTime, movieId, showtimeId);

				return Ok(new
				{
					success = true,
					data = availableRooms.Select(r => new { r.RoomId, r.Name })
				});
			}
			catch (Exception ex)
			{
				return BadRequest(new { success = false, message = ex.Message });
			}
		}
		#endregion
	}
}
