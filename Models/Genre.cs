using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models;

public partial class Genre
{
    public int GenreId { get; set; }

	[Display(Name = "Genre Name")] 
	[Required(ErrorMessage = "Genre Name is required")]
	public string GenreName { get; set; } = null!;

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
