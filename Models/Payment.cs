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
		public string? PamentIntentId { get; set; }
		public string? PaymentStatus { get; set; }
		public string? PaymentType { get; set; }
		public string? TrackingNumber { get; set; }
		public DateTime PaymentDate { get; set; }
		public DateOnly PaymentDueDate { get; set; }
		public virtual Booking Booking { get; set; }
	}
}
