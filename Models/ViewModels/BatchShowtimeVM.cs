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
	public class BatchShowtimeVM
	{
		[Required]
		[Display(Name = "Movie")]
		public int MovieId { get; set; }

		[Required]
		[Display(Name = "Start Date")]
		[DataType(DataType.Date)]
		public DateTime StartDate { get; set; } = DateTime.Today;

		[Required]
		[Display(Name = "End Date")]
		[DataType(DataType.Date)]
		public DateTime EndDate { get; set; } = DateTime.Today.AddDays(7);

		[Display(Name = "Selected Days")]
		[ValidateNever]
		public List<DayOfWeek> SelectedDays { get; set; } = new List<DayOfWeek>();

		[Display(Name = "Time Slots")]
		[ValidateNever]
		public List<TimeSpan> TimeSlots { get; set; } = new List<TimeSpan>();

		[Display(Name = "Preferred Rooms")]
		[ValidateNever]
		public List<int> PreferredRoomIds { get; set; } = new List<int>();

		[Display(Name = "Auto-resolve Conflicts")]
		public bool AutoResolveConflicts { get; set; } = true;

		// For display in the form
		[ValidateNever]
		public IEnumerable<SelectListItem> MovieList { get; set; }

		[ValidateNever]
		public IEnumerable<SelectListItem> RoomList { get; set; }

		// For results
		[ValidateNever]
		public int SuccessCount { get; set; }

		[ValidateNever]
		public int FailureCount { get; set; }

		[ValidateNever]
		public List<string> ErrorMessages { get; set; } = new List<string>();

		[ValidateNever]
		public List<Showtime> CreatedShowtimes { get; set; } = new List<Showtime>();
	}
}
