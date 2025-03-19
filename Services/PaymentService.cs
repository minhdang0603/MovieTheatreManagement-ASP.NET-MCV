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

		public void UpdateStripePaymentId(int paymentId, string sessionId, string paymentIntentId)
		{
			var payment = _context.Payments.Find(paymentId);
			if (payment == null)
			{
				throw new Exception("Payment not found");
			}
			if(!string.IsNullOrEmpty(sessionId))
			{
				payment.SessionId = sessionId;
			}
			if (!string.IsNullOrEmpty(paymentIntentId))
			{
				payment.PaymentIntentId = paymentIntentId;
				payment.PaymentDate = DateTime.Now;
			}
			_context.Payments.Update(payment);
		}

	}
}
