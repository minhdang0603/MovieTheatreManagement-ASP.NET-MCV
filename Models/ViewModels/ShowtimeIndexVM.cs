using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Models.ViewModels
{
    public class ShowtimeIndexVM
    {
        public List<Showtime> Showtimes { get; set; } = new List<Showtime>();
        public int? RoomId { get; set; }
        public int? MovieId { get; set; }
        public string SearchTerm { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> MovieList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> RoomList { get; set; }

        [ValidateNever]
        public PaginationVM PaginationInfo { get; set; }

    }
}
