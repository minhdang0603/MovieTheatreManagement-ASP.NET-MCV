using System;
using System.Collections.Generic;

namespace Models;

public partial class Director
{
    public int DirectorId { get; set; }

    public string DirectorName { get; set; } = null!;

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
