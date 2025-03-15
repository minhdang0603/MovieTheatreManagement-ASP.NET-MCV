﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
	public class ShowtimeVM
	{
		public Showtime Showtime { get; set; }
		[ValidateNever]
		public IEnumerable<SelectListItem> MovieList { get; set; }
		[ValidateNever]
		public IEnumerable<SelectListItem> RoomList { get; set; }
	}
}
