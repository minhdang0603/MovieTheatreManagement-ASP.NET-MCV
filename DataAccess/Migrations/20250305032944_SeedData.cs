using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "movie_id", "description", "director_id", "duration", "genre_id", "image_url", "release_date", "status_id", "title" },
                values: new object[,]
                {
                    { 1, "A mind-bending thriller.", 1, 148, 1, "", new DateOnly(2010, 7, 16), 1, "Inception" },
                    { 2, "A dark comedy crime film.", 2, 154, 2, "", new DateOnly(1994, 10, 14), 3, "Pulp Fiction" },
                    { 3, "Dinosaurs run wild in a theme park.", 3, 127, 5, "", new DateOnly(1993, 6, 11), 3, "Jurassic Park" }
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
                name: "IX_Movie_genre_id",
                table: "Movie",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_status_id",
                table: "Movie",
                column: "status_id");

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
                name: "Ticket");

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
                name: "Genre");

            migrationBuilder.DropTable(
                name: "MovieStatus");
        }
    }
}
