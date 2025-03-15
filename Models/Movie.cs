using System;
using System.Collections.Generic;

namespace Models;

public partial class Movie
{
    public int MovieId { get; set; }

    public string Title { get; set; } = null!;

    public int Duration { get; set; }

    public int? DirectorId { get; set; }

    public string? Description { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public string? ImageUrl { get; set; }

    public int? StatusId { get; set; }

    public virtual Director? Director { get; set; }

	public virtual ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();

	public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();

    public virtual MovieStatus? Status { get; set; }
}
