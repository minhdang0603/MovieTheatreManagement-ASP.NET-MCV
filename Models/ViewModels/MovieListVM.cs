using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class MovieListVM
    {
        public List<Movie> Movies { get; set; } = new List<Movie>();
        public List<Genre> Genres { get; set; } = new List<Genre>();
        public string CategoryName { get; set; }
        public int? StatusId { get; set; }
        public int? GenreId { get; set; }
        public string SearchQuery { get; set; }

        // Helper property to determine status color class
        public string StatusColorClass => StatusId == 1 ? "success" : "warning";
    }
}
