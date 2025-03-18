using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedShowtimeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Showtime_Movie",
                table: "Showtime");

            migrationBuilder.DropForeignKey(
                name: "FK_Showtime_Room",
                table: "Showtime");

            migrationBuilder.AlterColumn<int>(
                name: "room_id",
                table: "Showtime",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "movie_id",
                table: "Showtime",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Showtime_Movie",
                table: "Showtime",
                column: "movie_id",
                principalTable: "Movie",
                principalColumn: "movie_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Showtime_Room",
                table: "Showtime",
                column: "room_id",
                principalTable: "Room",
                principalColumn: "room_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Showtime_Movie",
                table: "Showtime");

            migrationBuilder.DropForeignKey(
                name: "FK_Showtime_Room",
                table: "Showtime");

            migrationBuilder.AlterColumn<int>(
                name: "room_id",
                table: "Showtime",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "movie_id",
                table: "Showtime",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Showtime_Movie",
                table: "Showtime",
                column: "movie_id",
                principalTable: "Movie",
                principalColumn: "movie_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Showtime_Room",
                table: "Showtime",
                column: "room_id",
                principalTable: "Room",
                principalColumn: "room_id");
        }
    }
}
