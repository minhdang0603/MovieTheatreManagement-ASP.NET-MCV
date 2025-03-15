using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
	public class RoomVM
	{
		public Room Room { get; set; }
		[ValidateNever]
		public List<int> SeatTypePerRow { get; set; } = new List<int>();
		[ValidateNever]
		public IEnumerable<SelectListItem> SeatTypeList { get; set; } = new List<SelectListItem>();

	}
}
