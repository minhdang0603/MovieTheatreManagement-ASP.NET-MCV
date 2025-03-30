using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utility;

namespace Models.ViewModels
{
    public class BookingIndexVM
    {
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public string SearchTerm { get; set; }
        public string Status { get; set; }
        public int? MovieId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> MovieList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> StatusList { get; set; }

        [ValidateNever]
        public PaginationVM PaginationInfo { get; set; }

        // Helper methods to get appropriate CSS classes for different statuses
        public string GetStatusBadgeClass(string status)
        {
            return status?.ToLower() switch
            {
                SD.Status_Reserve => "bg-warning",
                SD.Status_Paid => "bg-success",
                SD.Status_Cancelled => "bg-danger",
                _ => "bg-secondary"
            };
        }

        public string GetPaymentStatusBadgeClass(string status)
        {
            return status?.ToLower() switch
            {
                SD.Payment_Pending => "bg-warning",
                SD.Payment_Approved => "bg-success",
                SD.Payment_Rejected => "bg-danger",
                SD.Payment_Refunded => "bg-info",
                _ => "bg-secondary"
            };
        }
    }
}
