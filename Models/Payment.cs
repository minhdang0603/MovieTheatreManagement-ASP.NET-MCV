using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public partial class Payment
	{
		public int PaymentId { get; set; }
		public string? SessionId { get; set; }
		public string? PaymentIntentId { get; set; }
		public string? PaymentStatus { get; set; }
		public string? PaymentMethod { get; set; }
		public DateTime? PaymentDate { get; set; }
		public DateOnly PaymentDueDate { get; set; }

		public int BookingId { get; set; }

		public virtual Booking Booking { get; set; }
	}
}
