using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using Utility;

namespace DataAccess.Data;
public partial class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
	public ApplicationDbContext()
	{
	}

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

	public virtual DbSet<Booking> Bookings { get; set; }

	public virtual DbSet<Director> Directors { get; set; }

	public virtual DbSet<Genre> Genres { get; set; }

	public virtual DbSet<Movie> Movies { get; set; }

	public virtual DbSet<Movie> MovieGenres { get; set; }

	public virtual DbSet<MovieStatus> MovieStatuses { get; set; }

	public virtual DbSet<Room> Rooms { get; set; }

	public virtual DbSet<Seat> Seats { get; set; }

	public virtual DbSet<SeatType> SeatTypes { get; set; }

	public virtual DbSet<Showtime> Showtimes { get; set; }

	public virtual DbSet<Ticket> Tickets { get; set; }

	public virtual DbSet<Payment> Payments { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Payment>(entity =>
		{
			entity.HasKey(e => e.PaymentId);
			entity.ToTable("Payment");
			entity.Property(e => e.PaymentId).HasColumnName("payment_id");
			entity.Property(e => e.PaymentIntentId).HasColumnName("payment_intent_id");
			entity.Property(e => e.SessionId).HasColumnName("session_id");
			entity.Property(e => e.PaymentStatus).HasColumnName("payment_status").HasDefaultValue(SD.Payment_Pending);
			entity.Property(e => e.PaymentMethod).HasColumnName("payment_method").HasDefaultValue(SD.PaymentMethod_Cash);
			entity.Property(e => e.PaymentDate).HasColumnName("payment_date");
			entity.Property(e => e.PaymentDueDate).HasColumnName("payment_due_date");
			entity.Property(e => e.BookingId).HasColumnName("booking_id");

			entity.HasOne(p => p.Booking)
				.WithOne(b => b.Payment)
				.HasForeignKey<Payment>(p => p.BookingId)
				.HasConstraintName("FK_Payment_Booking");
		});

		modelBuilder.Entity<Booking>(entity =>
		{
			entity.HasKey(e => e.BookingId).HasName("PK__Booking__5DE3A5B19905FE69");

			entity.ToTable("Booking");

			entity.Property(e => e.BookingId).HasColumnName("booking_id");
			entity.Property(e => e.ShowtimeId).HasColumnName("showtime_id");
			entity.Property(e => e.Status)
				.IsUnicode(false)
				.HasDefaultValue("reserved")
				.HasColumnName("status");
			entity.Property(e => e.TotalPrice).HasColumnName("total_price");

			entity.HasOne(d => d.Showtime).WithMany(p => p.Bookings)
				.HasForeignKey(d => d.ShowtimeId)
				.HasConstraintName("FK_Booking_Showtime");


		});

		modelBuilder.Entity<Director>(entity =>
		{
			entity.HasKey(e => e.DirectorId).HasName("PK__Director__F5205E49E12CD953");

			entity.ToTable("Director");

			entity.Property(e => e.DirectorId).HasColumnName("director_id");
			entity.Property(e => e.DirectorName)
				.HasMaxLength(100)
				.IsUnicode(false)
				.HasColumnName("director_name");
		});

		modelBuilder.Entity<Genre>(entity =>
		{
			entity.HasKey(e => e.GenreId).HasName("PK__Genre__18428D425476C2F3");

			entity.ToTable("Genre");

			entity.Property(e => e.GenreId).HasColumnName("genre_id");
			entity.Property(e => e.GenreName)
				.HasMaxLength(255)
				.IsUnicode(false)
				.HasColumnName("genre_name");
		});

		modelBuilder.Entity<Movie>(entity =>
		{
			entity.HasKey(e => e.MovieId).HasName("PK__Movie__83CDF7491C0DF4C1");

			entity.ToTable("Movie");

			entity.Property(e => e.MovieId).HasColumnName("movie_id");
			entity.Property(e => e.Description)
				.HasMaxLength(500)
				.IsUnicode(false)
				.HasColumnName("description");
			entity.Property(e => e.DirectorId).HasColumnName("director_id");
			entity.Property(e => e.Duration).HasColumnName("duration");
			entity.Property(e => e.ImageUrl)
				.HasMaxLength(255)
				.IsUnicode(false)
				.HasColumnName("image_url");
			entity.Property(e => e.ReleaseDate).HasColumnName("release_date");
			entity.Property(e => e.StatusId).HasColumnName("status_id");
			entity.Property(e => e.Title)
				.HasMaxLength(255)
				.IsUnicode(false)
				.HasColumnName("title");

			entity.HasOne(d => d.Director).WithMany(p => p.Movies)
				.HasForeignKey(d => d.DirectorId)
				.HasConstraintName("FK_Movie_Director");

			entity.HasOne(d => d.Status).WithMany(p => p.Movies)
				.HasForeignKey(d => d.StatusId)
				.HasConstraintName("FK_Movie_Status");
		});

		modelBuilder.Entity<MovieGenre>(entity =>
		{
			entity.HasKey(mg => new { mg.MovieId, mg.GenreId });

			entity.ToTable("MovieGenre");

			entity.Property(e => e.GenreId).HasColumnName("genre_id");
			entity.Property(e => e.MovieId).HasColumnName("movie_id");

			entity.HasOne(mg => mg.Movie)
				.WithMany(g => g.MovieGenres)
				.HasForeignKey(mg => mg.MovieId);

			entity.HasOne(mg => mg.Genre)
				.WithMany(g => g.MovieGenres)
				.HasForeignKey(mg => mg.GenreId);
		});

		modelBuilder.Entity<MovieStatus>(entity =>
		{
			entity.HasKey(e => e.StatusId).HasName("PK__MovieSta__3683B531A6D57002");

			entity.ToTable("MovieStatus");

			entity.Property(e => e.StatusId).HasColumnName("status_id");
			entity.Property(e => e.StatusName)
				.HasMaxLength(255)
				.IsUnicode(false)
				.HasColumnName("status_name");
		});

		modelBuilder.Entity<Room>(entity =>
		{
			entity.HasKey(e => e.RoomId).HasName("PK__Room__19675A8A72E6B5AE");

			entity.ToTable("Room");

			entity.Property(e => e.RoomId).HasColumnName("room_id");
			entity.Property(e => e.Name)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasColumnName("name");
			entity.Property(e => e.TotalRows).HasColumnName("total_row");
			entity.Property(e => e.TotalColumns).HasColumnName("total_column");
		});

		modelBuilder.Entity<Seat>(entity =>
		{
			entity.HasKey(e => e.SeatId).HasName("PK__Seat__906DED9C543E62FF");

			entity.ToTable("Seat");

			entity.Property(e => e.SeatId).HasColumnName("seat_id");
			entity.Property(e => e.RoomId).HasColumnName("room_id");
			entity.Property(e => e.SeatColumn).HasColumnName("seat_column");
			entity.Property(e => e.SeatRow)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasColumnName("seat_row");
			entity.Property(e => e.TypeId).HasColumnName("type_id");

			entity.HasOne(d => d.Room).WithMany(p => p.Seats)
				.HasForeignKey(d => d.RoomId)
				.HasConstraintName("FK_Seat_Room");

			entity.HasOne(d => d.Type).WithMany(p => p.Seats)
				.HasForeignKey(d => d.TypeId)
				.HasConstraintName("FK_Seat_SeatType");
		});

		modelBuilder.Entity<SeatType>(entity =>
		{
			entity.HasKey(e => e.TypeId).HasName("PK__SeatType__2C00059808C7A841");

			entity.ToTable("SeatType");

			entity.Property(e => e.TypeId).HasColumnName("type_id");
			entity.Property(e => e.Price).HasColumnName("price");
			entity.Property(e => e.TypeName)
				.HasMaxLength(100)
				.IsUnicode(false)
				.HasColumnName("type_name");
		});

		modelBuilder.Entity<Showtime>(entity =>
		{
			entity.HasKey(e => e.ShowtimeId).HasName("PK__Showtime__A406B518E1190A6E");

			entity.ToTable("Showtime");

			entity.Property(e => e.ShowtimeId).HasColumnName("showtime_id");
			entity.Property(e => e.MovieId).HasColumnName("movie_id");
			entity.Property(e => e.RoomId).HasColumnName("room_id");
			entity.Property(e => e.StartTime)
				.HasColumnType("datetime")
				.HasColumnName("start_time");

			entity.HasOne(d => d.Movie).WithMany(p => p.Showtimes)
				.HasForeignKey(d => d.MovieId)
				.HasConstraintName("FK_Showtime_Movie");

			entity.HasOne(d => d.Room).WithMany(p => p.Showtimes)
				.HasForeignKey(d => d.RoomId)
				.HasConstraintName("FK_Showtime_Room");
		});

		modelBuilder.Entity<Ticket>(entity =>
		{
			entity.HasKey(e => e.TicketId).HasName("PK__Ticket__D596F96B98EA604E");

			entity.ToTable("Ticket");

			entity.Property(e => e.TicketId).HasColumnName("ticket_id");
			entity.Property(e => e.BookingId).HasColumnName("booking_id");
			entity.Property(e => e.SeatId).HasColumnName("seat_id");

			entity.HasOne(d => d.Booking).WithMany(p => p.Tickets)
				.HasForeignKey(d => d.BookingId)
				.HasConstraintName("FK_Ticket_Booking");

			entity.HasOne(d => d.Seat).WithMany(p => p.Tickets)
				.HasForeignKey(d => d.SeatId)
				.HasConstraintName("FK_Ticket_Seat");
		});

		modelBuilder.Entity<MovieStatus>().HasData(
		new MovieStatus { StatusId = 1, StatusName = "Now Showing" },
		new MovieStatus { StatusId = 2, StatusName = "Coming Soon" },
		new MovieStatus { StatusId = 3, StatusName = "Ended" }
	);

		modelBuilder.Entity<Genre>().HasData(
			new Genre { GenreId = 1, GenreName = "Action" },
			new Genre { GenreId = 2, GenreName = "Comedy" },
			new Genre { GenreId = 3, GenreName = "Drama" },
			new Genre { GenreId = 4, GenreName = "Horror" },
			new Genre { GenreId = 5, GenreName = "Science Fiction" }
		);

		modelBuilder.Entity<Director>().HasData(
			new Director { DirectorId = 1, DirectorName = "Christopher Nolan" },
			new Director { DirectorId = 2, DirectorName = "Quentin Tarantino" },
			new Director { DirectorId = 3, DirectorName = "Steven Spielberg" },
			new Director { DirectorId = 4, DirectorName = "Martin Scorsese" },
			new Director { DirectorId = 5, DirectorName = "James Cameron" }
		);

		modelBuilder.Entity<Movie>().HasData(
			new Movie
			{
				MovieId = 1,
				Title = "Inception",
				Duration = 148,
				DirectorId = 1,
				Description = "A mind-bending thriller.",
				ReleaseDate = new DateOnly(2010, 07, 16),
				ImageUrl = "",
				StatusId = 1
			},
			new Movie
			{
				MovieId = 2,
				Title = "Pulp Fiction",
				Duration = 154,
				DirectorId = 2,
				Description = "A dark comedy crime film.",
				ReleaseDate = new DateOnly(1994, 10, 14),
				ImageUrl = "",
				StatusId = 3
			},
			new Movie
			{
				MovieId = 3,
				Title = "Jurassic Park",
				Duration = 127,
				DirectorId = 3,
				Description = "Dinosaurs run wild in a theme park.",
				ReleaseDate = new DateOnly(1993, 06, 11),
				ImageUrl = "",
				StatusId = 3
			}
		);

		modelBuilder.Entity<Room>().HasData(
			new Room { RoomId = 1, Name = "IMAX Theater", TotalColumns = 10, TotalRows = 5 },
			new Room { RoomId = 2, Name = "Dolby Atmos", TotalColumns = 10, TotalRows = 8 },
			new Room { RoomId = 3, Name = "Standard Hall", TotalColumns = 10, TotalRows = 12 }
		);

		modelBuilder.Entity<SeatType>().HasData(
			new SeatType { TypeId = 1, TypeName = "Standard", Price = 10 },
			new SeatType { TypeId = 2, TypeName = "Premium", Price = 15 },
			new SeatType { TypeId = 3, TypeName = "VIP", Price = 20 }
		);

		modelBuilder.Entity<MovieGenre>().HasData(
			new MovieGenre { MovieId = 1, GenreId = 1 }, // Inception -> Action
			new MovieGenre { MovieId = 2, GenreId = 2 }, // Pulp Fiction -> Comedy
			new MovieGenre { MovieId = 3, GenreId = 5 }, // Jurassic Park -> Sci-Fi
			new MovieGenre { MovieId = 3, GenreId = 1 }  // Jurassic Park -> Action
		);

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
