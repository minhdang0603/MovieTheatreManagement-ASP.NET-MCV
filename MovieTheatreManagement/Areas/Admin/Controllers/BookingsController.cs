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
    public class BookingsController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        private const int ITEMS_PER_PAGE = 10;

        private int countPages { get; set; }
        public BookingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string searchTerm, string status, int? movieId, DateTime? startDate, DateTime? endDate, int page = 1)
        {
            page = page < 1 ? 1 : page;

            var bookings = _unitOfWork.Booking.GetPagedBookings(searchTerm, status, movieId, startDate, endDate, page, ITEMS_PER_PAGE);

            countPages = (int) Math.Ceiling((double)bookings.TotalCount / ITEMS_PER_PAGE);

            var bookingIndexVM = new BookingIndexVM
            {
                Bookings = bookings.Bookings,
                SearchTerm = searchTerm,
                Status = status,
                MovieId = movieId,
                StartDate = startDate,
                EndDate = endDate,
                MovieList = _unitOfWork.Movie.GetMovieList().Select(m => new SelectListItem
                {
                    Text = m.Title,
                    Value = m.MovieId.ToString()
                }),
                StatusList = new List<SelectListItem>
                {
                    new SelectListItem("All", ""),
                    new SelectListItem("Reserved", SD.Status_Reserve),
                    new SelectListItem("Paid", SD.Status_Paid),
                    new SelectListItem("Cancelled", SD.Status_Cancelled)
                },
                PaginationInfo = new PaginationVM
                {
                    currentPage = page,
                    countPages = countPages,
                    generateUrl = pageNumber =>
                    {
                        var url = Url.Action("Index", new
                        {
                            searchTerm,
                            status,
                            movieId,
                            startDate,
                            endDate,
                            page = pageNumber
                        });
                        return url;
                    }
                }
            };

            return View(bookingIndexVM);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = _unitOfWork.Booking.GetBookingWithDetails(id.Value);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePaymentStatus(int bookingId, string paymentStatus)
        {
            try
            {
                var booking = _unitOfWork.Booking.GetBookingWithDetails(bookingId);
                if (booking == null)
                {
                    return NotFound();
                }
                if (booking.Payment.PaymentMethod == SD.PaymentMethod_Cash && paymentStatus == SD.Payment_Approved)
                {
                    _unitOfWork.Booking.UpdateStatus(booking.Payment.BookingId, SD.Status_Paid);
                }
                else if (paymentStatus == SD.Payment_Rejected || paymentStatus == SD.Payment_Refunded)
                {
                    _unitOfWork.Booking.UpdateStatus(booking.Payment.BookingId, SD.Status_Cancelled);
                }
                _unitOfWork.Payment.UpdateStatus(booking.Payment.PaymentId, paymentStatus);
                _unitOfWork.Save();
                TempData["success"] = "Payment status updated successfully.";
                return RedirectToAction(nameof(Details), new { id = bookingId });
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error updating payment status: {ex.Message}";
                return RedirectToAction(nameof(Details), new { id = bookingId });
            }
        }
    }
}
