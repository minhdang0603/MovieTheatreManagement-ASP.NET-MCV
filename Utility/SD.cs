using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
	public static class SD
	{
		public const string SeatType_Standard = "Standard";
		public const string SeatType_VIP = "VIP";
		public const string SeatType_Premium = "Premium";
		public const string Role_Customer = "Customer";
		public const string Role_Employee = "Employee";
		public const string Role_Admin = "Admin";
		public const string Session_SelectedShowtimeId = "SelectedShowtimeId";
		public const string Session_SelectedSeatIds = "SelectedSeatIds";
		public const string Status_Reserve = "reserved";
		public const string Status_Paid = "paid";
		public const string Status_Cancelled = "cancelled";

		public const string Payment_Pending = "pending";
		public const string Payment_Approved = "approved";
		public const string Payment_Rejected = "rejected";

		public const string PaymentMethod_Cash = "cash";
		public const string PaymentMethod_CreditCard = "card";

		public const string Stripe_Paid = "paid";
	}
}
