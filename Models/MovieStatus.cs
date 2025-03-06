using System;
using System.Collections.Generic;

namespace Models;

public partial class MovieStatus
{
    public int StatusId { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
