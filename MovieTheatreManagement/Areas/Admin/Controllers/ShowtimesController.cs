using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.ViewModels;
using Newtonsoft.Json;
using Services.IService;
using Utility;

namespace MovieTheatreManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + ", " + SD.Role_Employee)]
    public class ShowtimesController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        private const int ITEMS_PER_PAGE = 10;

        private int countPages { get; set; }

        public ShowtimesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string searchTerm, int? movieId, int? roomId, DateTime? startDate, DateTime? endDate, int page = 1)
        {
            page = page < 1 ? 1 : page;
            var result = _unitOfWork.Showtime.GetShowtimeListPaginated(searchTerm, movieId, roomId, startDate, endDate, page, ITEMS_PER_PAGE);

            countPages = (int)Math.Ceiling(result.TotalCount / (double)ITEMS_PER_PAGE);

            var viewModel = new ShowtimeIndexVM
            {
                Showtimes = result.Showtimes,
                SearchTerm = searchTerm,
                MovieId = movieId,
                RoomId = roomId,
                StartDate = startDate,
                EndDate = endDate,
                MovieList = _unitOfWork.Movie.GetMovieList().Select(m => new SelectListItem
                {
                    Text = m.Title,
                    Value = m.MovieId.ToString()
                }),
                RoomList = _unitOfWork.Room.GetRoomList().Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.RoomId.ToString()
                }),
                PaginationInfo = new PaginationVM
                {
                    currentPage = page,
                    countPages = countPages,
                    generateUrl = pageNumber => Url.Action("Index", new
                    {
                        searchTerm,
                        movieId,
                        roomId,
                        startDate,
                        endDate,
                        page = pageNumber
                    })
                }
            };

            return View(viewModel);
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
        public IActionResult BatchCreate(BatchShowtimeVM batchVM)
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


            try
            {
                // Generate showtimes
                var result = _unitOfWork.Showtime.CreateBatchShowtimes(batchVM);

                // Save the results for the results page
                TempData["SuccessCount"] = result.CreatedShowtimes.Count;
                TempData["FailureCount"] = result.ErrorMessages.Count;

                // For complex objects, serialize to JSON
                TempData["ErrorMessages"] = JsonConvert.SerializeObject(result.ErrorMessages);

                // Store the IDs of created showtimes rather than the whole objects
                TempData["CreatedShowtimeIds"] = JsonConvert.SerializeObject(
                    result.CreatedShowtimes.Select(s => s.ShowtimeId).ToList());


                if (result.CreatedShowtimes.Count > 0)
                {
                    TempData["success"] = $"Successfully created {result.CreatedShowtimes.Count} showtimes.";
                }

                if (result.ErrorMessages.Count > 0)
                {
                    TempData["error"] = $"Failed to create {result.ErrorMessages.Count} showtimes. See details for more information.";
                }
                return RedirectToAction(nameof(BatchCreateResults));
            }
            catch (Exception ex)
            {
                batchVM.ErrorMessages.Add($"An error occurred: {ex.Message}");
                TempData["error"] = "An error occurred during batch creation.";
                return View(batchVM);
            }

        }

        public IActionResult BatchCreateResults()
        {
            if (TempData["SuccessCount"] == null && TempData["FailureCount"] == null)
            {
                return RedirectToAction(nameof(BatchCreate));
            }

            var batchVM = new BatchShowtimeVM();

            // Retrieve data from TempData
            batchVM.SuccessCount = (int)TempData["SuccessCount"];
            batchVM.FailureCount = (int)TempData["FailureCount"];

            // Deserialize complex objects
            if (TempData["ErrorMessages"] != null)
            {
                batchVM.ErrorMessages = JsonConvert.DeserializeObject<List<string>>(TempData["ErrorMessages"].ToString());
            }

            // Fetch showtimes by ID
            if (TempData["CreatedShowtimeIds"] != null)
            {
                var showtimeIds = JsonConvert.DeserializeObject<List<int>>(TempData["CreatedShowtimeIds"].ToString());
                batchVM.CreatedShowtimes = _unitOfWork.Showtime.GetShowtimeList()
                    .Where(s => showtimeIds.Contains(s.ShowtimeId))
                    .ToList();
            }

            return View(batchVM);

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
