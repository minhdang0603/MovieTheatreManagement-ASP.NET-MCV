using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using Models;
using Services.IService;
using System.Security.Claims;
using Utility;

namespace MovieTheatreManagement.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class BookingController : Controller
	{

		private readonly IUnitOfWork _unitOfWork;

		public BookingController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index(int? id)
		{
			// Get the showtime details
			var showtime = _unitOfWork.Showtime.GetShowtimeById(id.Value);
			if (showtime == null)
			{
				return NotFound();
			}

			// Check if the showtime is in the past
			if (showtime.StartTime < DateTime.Now)
			{
				TempData["error"] = "This showtime has already started or ended.";
				return RedirectToAction("Index", "Home");
			}

			// Get the movie details
			var movie = _unitOfWork.Movie.GetMovieById(showtime.MovieId);
			if (movie == null)
			{
				return NotFound();
			}

			// Get room details
			var room = _unitOfWork.Room.GetRoomById(showtime.RoomId);
			if (room == null)
			{
				return NotFound();
			}

			var seatTypes = _unitOfWork.SeatType.GetSeatTypeList();

			// Get all seats for the room
			var allSeats = room.Seats.ToList();

			// Get all booked tickets for this showtime to identify taken seats
			var bookedTickets = _unitOfWork.Showtime.GetBookedSeats(id.Value);

			// Create the view model
			var viewModel = new BookingVM
			{
				Showtime = showtime,
				Movie = movie,
				Room = room,
				SeatTypes = seatTypes,
				AllSeats = allSeats,
				BookedSeats = bookedTickets
			};

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult SelectSeats(int showtimeId, List<int> selectedSeatIds)
		{
			if (selectedSeatIds == null || !selectedSeatIds.Any())
			{
				TempData["error"] = "Please select at least one seat.";
				return RedirectToAction(nameof(Index), new { showtimeId });
			}

			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(userId))
			{
				HttpContext.Session.SetInt32(SD.Session_SelectedShowtimeId, showtimeId);
				HttpContext.Session.SetString(SD.Session_SelectedSeatIds, string.Join(",", selectedSeatIds));

				return RedirectToPage("/Account/Login", new { area = "Identity" });
			}

			HttpContext.Session.SetInt32(SD.Session_SelectedShowtimeId, showtimeId);
			HttpContext.Session.SetString(SD.Session_SelectedSeatIds, string.Join(",", selectedSeatIds));

			return RedirectToAction(nameof(Confirmation));
		}

		public IActionResult Confirmation()
		{
			int? showtimeId = HttpContext.Session.GetInt32(SD.Session_SelectedShowtimeId);
			string seatIds = HttpContext.Session.GetString(SD.Session_SelectedSeatIds);

			if (showtimeId == null ||
				string.IsNullOrEmpty(seatIds))
			{
				return RedirectToAction("Index", "Home");
			}

			List<int> selectedSeatIds = seatIds.Split(',').Select(int.Parse).ToList();

			var showtime = _unitOfWork.Showtime.GetShowtimeById(showtimeId.Value);
			var movie = _unitOfWork.Movie.GetMovieById(showtime.MovieId);
			var selectedSeats = _unitOfWork.Room.GetSeatsByIds(selectedSeatIds);

			decimal totalPrice = selectedSeats.Sum(s => s.Type?.Price ?? 0);

			var viewModel = new BookingConfirmationVM
			{
				Showtime = showtime,
				Movie = movie,
				SelectedSeats = selectedSeats,
				TotalPrice = totalPrice
			};

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult ConfirmBooking()
		{
			int? showtimeId = HttpContext.Session.GetInt32(SD.Session_SelectedShowtimeId);
			string seatIds = HttpContext.Session.GetString(SD.Session_SelectedSeatIds);

			if (showtimeId == null ||
				string.IsNullOrEmpty(seatIds))
			{
				return RedirectToAction("Index", "Home");
			}

			List<int> selectedSeatIds = seatIds.Split(',').Select(int.Parse).ToList();

			// Get current user ID
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(userId))
			{
				return RedirectToPage("/Account/Login", new { area = "Identity" });
			}

			try
			{
				// Create the booking
				var booking = new Booking
				{
					ShowtimeId = showtimeId,
					Status = SD.Status_Confirmed,
					ApplicationUserId = userId
				};

				// Add tickets for selected seats
				foreach (var seatId in selectedSeatIds)
				{
					booking.Tickets.Add(new Ticket
					{
						SeatId = seatId
					});
				}

				// Calculate total price
				var selectedSeats = _unitOfWork.Room.GetSeatsByIds(selectedSeatIds);
				booking.TotalPrice = (int)selectedSeats.Sum(s => s.Type?.Price ?? 0);

				// Save the booking
				_unitOfWork.Booking.CreateBooking(booking);
				HttpContext.Session.Clear();
				_unitOfWork.Save();

				TempData["success"] = "Booking confirmed successfully!";
				return RedirectToAction(nameof(BookingConfirmation), new { bookingId = booking.BookingId });
			}
			catch (Exception ex)
			{
				TempData["error"] = $"Error creating booking: {ex.Message}";
				return RedirectToAction("Index", "Home");
			}
		}

		public IActionResult BookingConfirmation(int bookingId)
		{
			var booking = _unitOfWork.Booking.GetBookingWithDetails(bookingId);
			if (booking == null)
			{
				return NotFound();
			}

			return View(booking);
		}

		public IActionResult MyBookings()
		{
			// Get current user ID
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(userId))
			{
				return RedirectToPage("/Account/Login", new { area = "Identity" });
			}

			var bookings = _unitOfWork.Booking.GetUserBookings(userId);
			return View(bookings);
		}

		#region API CALLS
		[HttpPut]
		public IActionResult CancelBooking(int id)
		{
			try
			{
				_unitOfWork.Booking.CancelBooking(id);
				_unitOfWork.Save();
				TempData["success"] = "Cancel booking successful";
			}
			catch (Exception ex)
			{
				TempData["error"] = ex.Message;
			}
			return RedirectToAction(nameof(MyBookings));
		}
		#endregion
	}
}
