﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
	public interface IBookingService
	{
		bool BookingExists(int bookingId);
		void CancelBooking(int bookingId);
		void CreateBooking(Booking booking);
		Booking GetBookingWithDetails(int bookingId);
        (List<Booking> Bookings, int TotalCount) GetPagedBookings(string searchTerm, string status, int? movieId, DateTime? startDate, DateTime? endDate, int pageIndex, int pageSize);
        List<Booking> GetUserBookings(string userId);
		void RemoveBooking(Booking booking);
		void UpdateStatus(int bookingId, string status);
	}
}
