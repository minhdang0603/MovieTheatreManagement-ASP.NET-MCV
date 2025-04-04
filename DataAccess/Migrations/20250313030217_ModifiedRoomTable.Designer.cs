﻿// <auto-generated />
using System;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250313030217_ModifiedRoomTable")]
    partial class ModifiedRoomTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Models.Booking", b =>
                {
                    b.Property<int>("BookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("booking_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingId"));

                    b.Property<int?>("ShowtimeId")
                        .HasColumnType("int")
                        .HasColumnName("showtime_id");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasDefaultValue("reserved")
                        .HasColumnName("status");

                    b.Property<int?>("TotalPrice")
                        .HasColumnType("int")
                        .HasColumnName("total_price");

                    b.HasKey("BookingId")
                        .HasName("PK__Booking__5DE3A5B19905FE69");

                    b.HasIndex("ShowtimeId");

                    b.ToTable("Booking", (string)null);
                });

            modelBuilder.Entity("Models.Director", b =>
                {
                    b.Property<int>("DirectorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("director_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DirectorId"));

                    b.Property<string>("DirectorName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("director_name");

                    b.HasKey("DirectorId")
                        .HasName("PK__Director__F5205E49E12CD953");

                    b.ToTable("Director", (string)null);

                    b.HasData(
                        new
                        {
                            DirectorId = 1,
                            DirectorName = "Christopher Nolan"
                        },
                        new
                        {
                            DirectorId = 2,
                            DirectorName = "Quentin Tarantino"
                        },
                        new
                        {
                            DirectorId = 3,
                            DirectorName = "Steven Spielberg"
                        },
                        new
                        {
                            DirectorId = 4,
                            DirectorName = "Martin Scorsese"
                        },
                        new
                        {
                            DirectorId = 5,
                            DirectorName = "James Cameron"
                        });
                });

            modelBuilder.Entity("Models.Genre", b =>
                {
                    b.Property<int>("GenreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("genre_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GenreId"));

                    b.Property<string>("GenreName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("genre_name");

                    b.HasKey("GenreId")
                        .HasName("PK__Genre__18428D425476C2F3");

                    b.ToTable("Genre", (string)null);

                    b.HasData(
                        new
                        {
                            GenreId = 1,
                            GenreName = "Action"
                        },
                        new
                        {
                            GenreId = 2,
                            GenreName = "Comedy"
                        },
                        new
                        {
                            GenreId = 3,
                            GenreName = "Drama"
                        },
                        new
                        {
                            GenreId = 4,
                            GenreName = "Horror"
                        },
                        new
                        {
                            GenreId = 5,
                            GenreName = "Science Fiction"
                        });
                });

            modelBuilder.Entity("Models.Movie", b =>
                {
                    b.Property<int>("MovieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("movie_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MovieId"));

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("description");

                    b.Property<int?>("DirectorId")
                        .HasColumnType("int")
                        .HasColumnName("director_id");

                    b.Property<int>("Duration")
                        .HasColumnType("int")
                        .HasColumnName("duration");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("image_url");

                    b.Property<DateOnly?>("ReleaseDate")
                        .HasColumnType("date")
                        .HasColumnName("release_date");

                    b.Property<int?>("StatusId")
                        .HasColumnType("int")
                        .HasColumnName("status_id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("title");

                    b.HasKey("MovieId")
                        .HasName("PK__Movie__83CDF7491C0DF4C1");

                    b.HasIndex("DirectorId");

                    b.HasIndex("StatusId");

                    b.ToTable("Movie", (string)null);

                    b.HasData(
                        new
                        {
                            MovieId = 1,
                            Description = "A mind-bending thriller.",
                            DirectorId = 1,
                            Duration = 148,
                            ImageUrl = "",
                            ReleaseDate = new DateOnly(2010, 7, 16),
                            StatusId = 1,
                            Title = "Inception"
                        },
                        new
                        {
                            MovieId = 2,
                            Description = "A dark comedy crime film.",
                            DirectorId = 2,
                            Duration = 154,
                            ImageUrl = "",
                            ReleaseDate = new DateOnly(1994, 10, 14),
                            StatusId = 3,
                            Title = "Pulp Fiction"
                        },
                        new
                        {
                            MovieId = 3,
                            Description = "Dinosaurs run wild in a theme park.",
                            DirectorId = 3,
                            Duration = 127,
                            ImageUrl = "",
                            ReleaseDate = new DateOnly(1993, 6, 11),
                            StatusId = 3,
                            Title = "Jurassic Park"
                        });
                });

            modelBuilder.Entity("Models.MovieGenre", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("int")
                        .HasColumnName("movie_id");

                    b.Property<int>("GenreId")
                        .HasColumnType("int")
                        .HasColumnName("genre_id");

                    b.HasKey("MovieId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("MovieGenre", (string)null);

                    b.HasData(
                        new
                        {
                            MovieId = 1,
                            GenreId = 1
                        },
                        new
                        {
                            MovieId = 2,
                            GenreId = 2
                        },
                        new
                        {
                            MovieId = 3,
                            GenreId = 5
                        },
                        new
                        {
                            MovieId = 3,
                            GenreId = 1
                        });
                });

            modelBuilder.Entity("Models.MovieStatus", b =>
                {
                    b.Property<int>("StatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("status_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StatusId"));

                    b.Property<string>("StatusName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("status_name");

                    b.HasKey("StatusId")
                        .HasName("PK__MovieSta__3683B531A6D57002");

                    b.ToTable("MovieStatus", (string)null);

                    b.HasData(
                        new
                        {
                            StatusId = 1,
                            StatusName = "Now Showing"
                        },
                        new
                        {
                            StatusId = 2,
                            StatusName = "Coming Soon"
                        },
                        new
                        {
                            StatusId = 3,
                            StatusName = "Ended"
                        });
                });

            modelBuilder.Entity("Models.Room", b =>
                {
                    b.Property<int>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("room_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoomId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.Property<int>("TotalColumns")
                        .HasColumnType("int")
                        .HasColumnName("total_column");

                    b.Property<int>("TotalRows")
                        .HasColumnType("int")
                        .HasColumnName("total_row");

                    b.HasKey("RoomId")
                        .HasName("PK__Room__19675A8A72E6B5AE");

                    b.ToTable("Room", (string)null);

                    b.HasData(
                        new
                        {
                            RoomId = 1,
                            Name = "IMAX Theater",
                            TotalColumns = 10,
                            TotalRows = 5
                        },
                        new
                        {
                            RoomId = 2,
                            Name = "Dolby Atmos",
                            TotalColumns = 10,
                            TotalRows = 8
                        },
                        new
                        {
                            RoomId = 3,
                            Name = "Standard Hall",
                            TotalColumns = 10,
                            TotalRows = 12
                        });
                });

            modelBuilder.Entity("Models.Seat", b =>
                {
                    b.Property<int>("SeatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("seat_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SeatId"));

                    b.Property<int?>("RoomId")
                        .HasColumnType("int")
                        .HasColumnName("room_id");

                    b.Property<int>("SeatColumn")
                        .HasColumnType("int")
                        .HasColumnName("seat_column");

                    b.Property<string>("SeatRow")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("seat_row");

                    b.Property<int?>("TypeId")
                        .HasColumnType("int")
                        .HasColumnName("type_id");

                    b.HasKey("SeatId")
                        .HasName("PK__Seat__906DED9C543E62FF");

                    b.HasIndex("RoomId");

                    b.HasIndex("TypeId");

                    b.ToTable("Seat", (string)null);
                });

            modelBuilder.Entity("Models.SeatType", b =>
                {
                    b.Property<int>("TypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("type_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TypeId"));

                    b.Property<int?>("Price")
                        .HasColumnType("int")
                        .HasColumnName("price");

                    b.Property<string>("TypeName")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("type_name");

                    b.HasKey("TypeId")
                        .HasName("PK__SeatType__2C00059808C7A841");

                    b.ToTable("SeatType", (string)null);

                    b.HasData(
                        new
                        {
                            TypeId = 1,
                            Price = 10,
                            TypeName = "Standard"
                        },
                        new
                        {
                            TypeId = 2,
                            Price = 15,
                            TypeName = "Premium"
                        },
                        new
                        {
                            TypeId = 3,
                            Price = 20,
                            TypeName = "VIP"
                        });
                });

            modelBuilder.Entity("Models.Showtime", b =>
                {
                    b.Property<int>("ShowtimeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("showtime_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShowtimeId"));

                    b.Property<int?>("MovieId")
                        .HasColumnType("int")
                        .HasColumnName("movie_id");

                    b.Property<int?>("RoomId")
                        .HasColumnType("int")
                        .HasColumnName("room_id");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime")
                        .HasColumnName("start_time");

                    b.HasKey("ShowtimeId")
                        .HasName("PK__Showtime__A406B518E1190A6E");

                    b.HasIndex("MovieId");

                    b.HasIndex("RoomId");

                    b.ToTable("Showtime", (string)null);
                });

            modelBuilder.Entity("Models.Ticket", b =>
                {
                    b.Property<int>("TicketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ticket_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TicketId"));

                    b.Property<int?>("BookingId")
                        .HasColumnType("int")
                        .HasColumnName("booking_id");

                    b.Property<int?>("SeatId")
                        .HasColumnType("int")
                        .HasColumnName("seat_id");

                    b.HasKey("TicketId")
                        .HasName("PK__Ticket__D596F96B98EA604E");

                    b.HasIndex("BookingId");

                    b.HasIndex("SeatId");

                    b.ToTable("Ticket", (string)null);
                });

            modelBuilder.Entity("Models.Booking", b =>
                {
                    b.HasOne("Models.Showtime", "Showtime")
                        .WithMany("Bookings")
                        .HasForeignKey("ShowtimeId")
                        .HasConstraintName("FK_Booking_Showtime");

                    b.Navigation("Showtime");
                });

            modelBuilder.Entity("Models.Movie", b =>
                {
                    b.HasOne("Models.Director", "Director")
                        .WithMany("Movies")
                        .HasForeignKey("DirectorId")
                        .HasConstraintName("FK_Movie_Director");

                    b.HasOne("Models.MovieStatus", "Status")
                        .WithMany("Movies")
                        .HasForeignKey("StatusId")
                        .HasConstraintName("FK_Movie_Status");

                    b.Navigation("Director");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("Models.MovieGenre", b =>
                {
                    b.HasOne("Models.Genre", "Genre")
                        .WithMany("MovieGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Movie", "Movie")
                        .WithMany("MovieGenres")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("Models.Seat", b =>
                {
                    b.HasOne("Models.Room", "Room")
                        .WithMany("Seats")
                        .HasForeignKey("RoomId")
                        .HasConstraintName("FK_Seat_Room");

                    b.HasOne("Models.SeatType", "Type")
                        .WithMany("Seats")
                        .HasForeignKey("TypeId")
                        .HasConstraintName("FK_Seat_SeatType");

                    b.Navigation("Room");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("Models.Showtime", b =>
                {
                    b.HasOne("Models.Movie", "Movie")
                        .WithMany("Showtimes")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("FK_Showtime_Movie");

                    b.HasOne("Models.Room", "Room")
                        .WithMany("Showtimes")
                        .HasForeignKey("RoomId")
                        .HasConstraintName("FK_Showtime_Room");

                    b.Navigation("Movie");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Models.Ticket", b =>
                {
                    b.HasOne("Models.Booking", "Booking")
                        .WithMany("Tickets")
                        .HasForeignKey("BookingId")
                        .HasConstraintName("FK_Ticket_Booking");

                    b.HasOne("Models.Seat", "Seat")
                        .WithMany("Tickets")
                        .HasForeignKey("SeatId")
                        .HasConstraintName("FK_Ticket_Seat");

                    b.Navigation("Booking");

                    b.Navigation("Seat");
                });

            modelBuilder.Entity("Models.Booking", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("Models.Director", b =>
                {
                    b.Navigation("Movies");
                });

            modelBuilder.Entity("Models.Genre", b =>
                {
                    b.Navigation("MovieGenres");
                });

            modelBuilder.Entity("Models.Movie", b =>
                {
                    b.Navigation("MovieGenres");

                    b.Navigation("Showtimes");
                });

            modelBuilder.Entity("Models.MovieStatus", b =>
                {
                    b.Navigation("Movies");
                });

            modelBuilder.Entity("Models.Room", b =>
                {
                    b.Navigation("Seats");

                    b.Navigation("Showtimes");
                });

            modelBuilder.Entity("Models.Seat", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("Models.SeatType", b =>
                {
                    b.Navigation("Seats");
                });

            modelBuilder.Entity("Models.Showtime", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
