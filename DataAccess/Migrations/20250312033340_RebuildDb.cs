using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RebuildDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Director",
                columns: table => new
                {
                    director_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    director_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Director__F5205E49E12CD953", x => x.director_id);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    genre_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    genre_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Genre__18428D425476C2F3", x => x.genre_id);
                });

            migrationBuilder.CreateTable(
                name: "MovieStatus",
                columns: table => new
                {
                    status_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MovieSta__3683B531A6D57002", x => x.status_id);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    room_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    seats_no = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Room__19675A8A72E6B5AE", x => x.room_id);
                });

            migrationBuilder.CreateTable(
                name: "SeatType",
                columns: table => new
                {
                    type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    price = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SeatType__2C00059808C7A841", x => x.type_id);
                });

            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    movie_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    duration = table.Column<int>(type: "int", nullable: false),
                    director_id = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    release_date = table.Column<DateOnly>(type: "date", nullable: true),
                    image_url = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Movie__83CDF7491C0DF4C1", x => x.movie_id);
                    table.ForeignKey(
                        name: "FK_Movie_Director",
                        column: x => x.director_id,
                        principalTable: "Director",
                        principalColumn: "director_id");
                    table.ForeignKey(
                        name: "FK_Movie_Status",
                        column: x => x.status_id,
                        principalTable: "MovieStatus",
                        principalColumn: "status_id");
                });

            migrationBuilder.CreateTable(
                name: "Seat",
                columns: table => new
                {
                    seat_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    room_id = table.Column<int>(type: "int", nullable: true),
                    type_id = table.Column<int>(type: "int", nullable: true),
                    seat_row = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    seat_column = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Seat__906DED9C543E62FF", x => x.seat_id);
                    table.ForeignKey(
                        name: "FK_Seat_Room",
                        column: x => x.room_id,
                        principalTable: "Room",
                        principalColumn: "room_id");
                    table.ForeignKey(
                        name: "FK_Seat_SeatType",
                        column: x => x.type_id,
                        principalTable: "SeatType",
                        principalColumn: "type_id");
                });

            migrationBuilder.CreateTable(
                name: "MovieGenre",
                columns: table => new
                {
                    movie_id = table.Column<int>(type: "int", nullable: false),
                    genre_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieGenre", x => new { x.movie_id, x.genre_id });
                    table.ForeignKey(
                        name: "FK_MovieGenre_Genre_genre_id",
                        column: x => x.genre_id,
                        principalTable: "Genre",
                        principalColumn: "genre_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieGenre_Movie_movie_id",
                        column: x => x.movie_id,
                        principalTable: "Movie",
                        principalColumn: "movie_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Showtime",
                columns: table => new
                {
                    showtime_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    movie_id = table.Column<int>(type: "int", nullable: true),
                    room_id = table.Column<int>(type: "int", nullable: true),
                    start_time = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Showtime__A406B518E1190A6E", x => x.showtime_id);
                    table.ForeignKey(
                        name: "FK_Showtime_Movie",
                        column: x => x.movie_id,
                        principalTable: "Movie",
                        principalColumn: "movie_id");
                    table.ForeignKey(
                        name: "FK_Showtime_Room",
                        column: x => x.room_id,
                        principalTable: "Room",
                        principalColumn: "room_id");
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    booking_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    showtime_id = table.Column<int>(type: "int", nullable: true),
                    total_price = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true, defaultValue: "reserved")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Booking__5DE3A5B19905FE69", x => x.booking_id);
                    table.ForeignKey(
                        name: "FK_Booking_Showtime",
                        column: x => x.showtime_id,
                        principalTable: "Showtime",
                        principalColumn: "showtime_id");
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    ticket_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    seat_id = table.Column<int>(type: "int", nullable: true),
                    booking_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ticket__D596F96B98EA604E", x => x.ticket_id);
                    table.ForeignKey(
                        name: "FK_Ticket_Booking",
                        column: x => x.booking_id,
                        principalTable: "Booking",
                        principalColumn: "booking_id");
                    table.ForeignKey(
                        name: "FK_Ticket_Seat",
                        column: x => x.seat_id,
                        principalTable: "Seat",
                        principalColumn: "seat_id");
                });

            migrationBuilder.InsertData(
                table: "Director",
                columns: new[] { "director_id", "director_name" },
                values: new object[,]
                {
                    { 1, "Christopher Nolan" },
                    { 2, "Quentin Tarantino" },
                    { 3, "Steven Spielberg" },
                    { 4, "Martin Scorsese" },
                    { 5, "James Cameron" }
                });

            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "genre_id", "genre_name" },
                values: new object[,]
                {
                    { 1, "Action" },
                    { 2, "Comedy" },
                    { 3, "Drama" },
                    { 4, "Horror" },
                    { 5, "Science Fiction" }
                });

            migrationBuilder.InsertData(
                table: "MovieStatus",
                columns: new[] { "status_id", "status_name" },
                values: new object[,]
                {
                    { 1, "Now Showing" },
                    { 2, "Coming Soon" },
                    { 3, "Ended" }
                });

            migrationBuilder.InsertData(
                table: "Room",
                columns: new[] { "room_id", "name", "seats_no" },
                values: new object[,]
                {
                    { 1, "IMAX Theater", 100 },
                    { 2, "Dolby Atmos", 80 },
                    { 3, "Standard Hall", 120 }
                });

            migrationBuilder.InsertData(
                table: "SeatType",
                columns: new[] { "type_id", "price", "type_name" },
                values: new object[,]
                {
                    { 1, 10, "Standard" },
                    { 2, 15, "Premium" },
                    { 3, 20, "VIP" }
                });

            migrationBuilder.InsertData(
                table: "Movie",
                columns: new[] { "movie_id", "description", "director_id", "duration", "image_url", "release_date", "status_id", "title" },
                values: new object[,]
                {
                    { 1, "A mind-bending thriller.", 1, 148, "", new DateOnly(2010, 7, 16), 1, "Inception" },
                    { 2, "A dark comedy crime film.", 2, 154, "", new DateOnly(1994, 10, 14), 3, "Pulp Fiction" },
                    { 3, "Dinosaurs run wild in a theme park.", 3, 127, "", new DateOnly(1993, 6, 11), 3, "Jurassic Park" }
                });

            migrationBuilder.InsertData(
                table: "MovieGenre",
                columns: new[] { "genre_id", "movie_id" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 1, 3 },
                    { 5, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_showtime_id",
                table: "Booking",
                column: "showtime_id");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_director_id",
                table: "Movie",
                column: "director_id");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_status_id",
                table: "Movie",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenre_genre_id",
                table: "MovieGenre",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_room_id",
                table: "Seat",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_type_id",
                table: "Seat",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Showtime_movie_id",
                table: "Showtime",
                column: "movie_id");

            migrationBuilder.CreateIndex(
                name: "IX_Showtime_room_id",
                table: "Showtime",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_booking_id",
                table: "Ticket",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_seat_id",
                table: "Ticket",
                column: "seat_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieGenre");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Seat");

            migrationBuilder.DropTable(
                name: "Showtime");

            migrationBuilder.DropTable(
                name: "SeatType");

            migrationBuilder.DropTable(
                name: "Movie");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "Director");

            migrationBuilder.DropTable(
                name: "MovieStatus");
        }
    }
}
