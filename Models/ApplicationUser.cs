using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.CustomValidations;

namespace Models
{
	public class ApplicationUser : IdentityUser
	{
		[Required]
		public string Name { get; set; }

		[Required]
		[DataType(DataType.Date)]
		[AgeValidation(13, 120)]
		public DateOnly DateOfBirth { get; set; }
	}
}
