using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.ViewModels;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Services
{
	public class BookingService : IBookingService
	{
		private readonly ApplicationDbContext _context;

		public BookingService(ApplicationDbContext context)
		{
			_context = context;
		}

		public void CreateBooking(Booking booking)
		{
			// Validate booking
			if (booking.ShowtimeId == null)
			{
				throw new Exception("Showtime is required for booking");
			}

			if (booking.Tickets == null || !booking.Tickets.Any())
			{
				throw new Exception("At least one ticket is required for booking");
			}

			// Check if showtime exists
			var showtime = _context.Showtimes.Find(booking.ShowtimeId);
			if (showtime == null)
			{
				throw new Exception("Showtime not found");
			}

			// Check if showtime is in the past
			if (showtime.StartTime < DateTime.Now)
			{
				throw new Exception("Cannot book for a showtime that has already started");
			}

			// Check if any of the selected seats are already booked
			var selectedSeatIds = booking.Tickets.Select(t => t.SeatId).ToList();
			var bookedSeatIds = _context.Bookings
				.Where(b => b.ShowtimeId == booking.ShowtimeId && b.Status != SD.Status_Cancelled)
				.SelectMany(b => b.Tickets)
				.Select(t => t.SeatId)
				.ToList();

			var alreadyBookedSeats = selectedSeatIds.Intersect(bookedSeatIds).ToList();
			if (alreadyBookedSeats.Any())
			{
				throw new Exception("Some of the selected seats are already booked");
			}

			// Add booking
			_context.Bookings.Add(booking);
		}

		public Booking GetBookingWithDetails(int bookingId)
		{
			return _context.Bookings
				.Include(b => b.Showtime)!
					.ThenInclude(s => s.Movie)
				.Include(b => b.Showtime)
					.ThenInclude(s => s.Room)
				.Include(b => b.Tickets)
					.ThenInclude(t => t.Seat)
				.Include(b => b.Payment)
				.FirstOrDefault(b => b.BookingId == bookingId);
		}

		public List<Booking> GetUserBookings(string userId)
		{
			return _context.Bookings
				.Where(b => b.ApplicationUserId == userId)
				.Include(b => b.Showtime)
					.ThenInclude(s => s.Movie)
				.Include(b => b.Showtime)
					.ThenInclude(s => s.Room)
				.Include(b => b.Tickets)
					.ThenInclude(t => t.Seat)
				.OrderByDescending(b => b.Showtime.StartTime)
				.ToList();
		}

		public void CancelBooking(int bookingId)
		{
			var booking = _context.Bookings.Find(bookingId);
			if (booking == null)
			{
				throw new Exception("Booking not found");
			}

			// Check if the showtime is in the past
			var showtime = _context.Showtimes.Find(booking.ShowtimeId);
			if (showtime != null && showtime.StartTime < DateTime.Now)
			{
				throw new Exception("Cannot cancel a booking for a showtime that has already started");
			}

			// Update booking status
			booking.Status = SD.Status_Cancelled;
			_context.Bookings.Update(booking);
		}

		public bool BookingExists(int bookingId)
		{
			return _context.Bookings.Any(b => b.BookingId == bookingId);
		}

		public void UpdateStatus(int bookingId, string status)
		{
			var booking = _context.Bookings.Find(bookingId);
			if (booking == null)
			{
				throw new Exception("Booking not found");
			}
			booking.Status = status;
			_context.Bookings.Update(booking);
		}

		public void RemoveBooking(Booking booking)
		{
			_context.Bookings.Remove(booking);
		}

	}
}
