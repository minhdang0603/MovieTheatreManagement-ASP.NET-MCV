using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class PaginationVM
    {
        public int currentPage { get; set; }

        public int countPages { get; set; }

        public Func<int?, string> generateUrl { get; set; }
    }
}
