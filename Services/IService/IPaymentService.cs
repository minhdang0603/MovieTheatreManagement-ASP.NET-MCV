﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
	public interface IPaymentService
	{
		void CreatePayment(Payment payment);
		void UpdateStatus(int paymentId, string status);
	}
}
