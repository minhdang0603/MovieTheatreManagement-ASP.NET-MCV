using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
	public class MovieVM
	{
		public Movie Movie { get; set; }
		[ValidateNever]
		public IEnumerable<SelectListItem> GenreList { get; set; }
		[ValidateNever]
		public IEnumerable<SelectListItem> DirectorList { get; set; }
		[ValidateNever]
		public IEnumerable<SelectListItem> StatusList { get; set; }
	}
}
