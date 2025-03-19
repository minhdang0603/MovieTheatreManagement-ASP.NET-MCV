using DataAccess.Data;
using Models;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class PaymentService : IPaymentService
	{

		private readonly ApplicationDbContext _context;

		public PaymentService(ApplicationDbContext context)
		{
			_context = context;
		}

		public Payment GetPaymentByBookingId(int bookingId)
		{
			return _context.Payments.FirstOrDefault(p => p.BookingId == bookingId);
		}

		public Payment GetPaymentBySessionId(string sessionId)
		{
			return _context.Payments.FirstOrDefault(p => p.SessionId == sessionId);
		}

		public void CreatePayment(Payment payment)
		{
			_context.Payments.Add(payment);
		}

		public void UpdateStatus(int paymentId, string status)
		{
			var payment = _context.Payments.Find(paymentId);
			if (payment == null)
			{
				throw new Exception("Payment not found");
			}
			payment.PaymentStatus = status;
			_context.Payments.Update(payment);
		}

		public void UpdatePayment(Payment payment)
		{
			_context.Payments.Update(payment);
		}

	}
}
