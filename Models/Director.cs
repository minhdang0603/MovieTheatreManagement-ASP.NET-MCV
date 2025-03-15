using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Models;

public partial class Director
{
    public int DirectorId { get; set; }

    [DisplayName("Director Name")]
    public string DirectorName { get; set; } = null!;

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
