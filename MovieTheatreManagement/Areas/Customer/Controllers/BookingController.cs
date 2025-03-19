using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using Models;
using Services.IService;
using System.Security.Claims;
using Utility;
using Stripe;

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
		public IActionResult ConfirmBooking(string paymentMethod)
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
					Status = SD.Status_Reserve,
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

				_unitOfWork.Booking.CreateBooking(booking);
				HttpContext.Session.Clear();
				_unitOfWork.Save();

				if (paymentMethod == SD.PaymentMethod_Cash)
				{
					var cashPayment = new Payment
					{
						PaymentMethod = paymentMethod,
						PaymentStatus = SD.Payment_Pending,
						PaymentDueDate = DateOnly.FromDateTime(_unitOfWork.Showtime.GetShowtimeById(booking.ShowtimeId.Value).StartTime),
						BookingId = booking.BookingId
					};

					_unitOfWork.Payment.CreatePayment(cashPayment);
					_unitOfWork.Save();
					TempData["success"] = "Booking confirmed successfully!";
					return RedirectToAction(nameof(BookingConfirmation), new { bookingId = booking.BookingId });
				}

				var domain = $"{Request.Scheme}://{Request.Host}"; ;
				var options = new Stripe.Checkout.SessionCreateOptions
				{
					PaymentMethodTypes = new List<string> { "card" },
					LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
				{
					new Stripe.Checkout.SessionLineItemOptions
					{
						PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
						{
							UnitAmount = booking.TotalPrice * 100, // Stripe uses cents
                            Currency = "usd",
							ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
							{
								Name = "Movie Tickets",
								Description = $"Tickets for {_unitOfWork.Showtime.GetShowtimeById(showtimeId.Value).Movie.Title}"
							}
						},
						Quantity = 1
					},
				},
					Mode = "payment",
					SuccessUrl = domain + $"/Customer/Booking/PaymentSuccess?bookingId={booking.BookingId}",
					CancelUrl = domain + $"/Customer/Booking/PaymentCancel?bookingId={booking.BookingId}"
				};
				var service = new Stripe.Checkout.SessionService();
				var session = service.Create(options);

				var stripePayment = new Payment
				{
					PaymentMethod = paymentMethod,
					PaymentStatus = SD.Payment_Pending,
					SessionId = session.Id,
					PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
					BookingId = booking.BookingId
				};

				_unitOfWork.Payment.CreatePayment(stripePayment);
				_unitOfWork.Save();

				return Redirect(session.Url);
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

		public IActionResult PaymentSuccess(int bookingId)
		{
			try
			{
				var booking = _unitOfWork.Booking.GetBookingWithDetails(bookingId);
				if (booking == null)
				{
					TempData["error"] = "Booking not found";
					return RedirectToAction("Index", "Home");
				}

				// Get the payment
				var payment = _unitOfWork.Payment.GetPaymentByBookingId(bookingId);
				if (payment == null)
				{
					TempData["error"] = "Payment not found";
					return RedirectToAction("Index", "Home");
				}

				// Verify payment status with Stripe
				var sessionService = new Stripe.Checkout.SessionService();
				var session = sessionService.Get(payment.SessionId);

				if (session.PaymentStatus == SD.Stripe_Paid)
				{
					payment.PaymentDate = DateTime.Now;
					payment.PaymentStatus = SD.Payment_Approved;
					payment.PaymentIntentId = session.PaymentIntentId;
					_unitOfWork.Booking.UpdateStatus(bookingId, SD.Status_Paid);
					_unitOfWork.Payment.UpdatePayment(payment);
					_unitOfWork.Save();

					TempData["success"] = "Payment successful! Your booking is confirmed.";
				}
				else
				{
					TempData["error"] = "Payment pending or failed. Please contact support.";
				}

				return RedirectToAction(nameof(BookingConfirmation), new { bookingId = bookingId });
			}
			catch (Exception ex)
			{
				TempData["error"] = $"Error processing payment: {ex.Message}";
				return RedirectToAction("Index", "Home");
			}
		}

		public IActionResult PaymentCancel(int bookingId)
		{
			try
			{
				// Get the booking and update status
				var booking = _unitOfWork.Booking.GetBookingWithDetails(bookingId);
				var payment = _unitOfWork.Payment.GetPaymentByBookingId(bookingId);
				if (booking != null && payment != null)
				{
					_unitOfWork.Booking.UpdateStatus(bookingId, SD.Status_PaymentFailed);
					_unitOfWork.Payment.UpdateStatus(payment.PaymentId, SD.Payment_Rejected);
					_unitOfWork.Save();
				}

				TempData["error"] = "Payment was cancelled. Your booking is not confirmed.";
				return RedirectToAction(nameof(BookingConfirmation), new { bookingId = bookingId });
			}
			catch (Exception ex)
			{
				TempData["error"] = $"Error cancelling payment: {ex.Message}";
				return RedirectToAction("Index", "Home");
			}
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
