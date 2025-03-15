using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedSeatTableVer20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seat_Room",
                table: "Seat");

            migrationBuilder.AlterColumn<int>(
                name: "room_id",
                table: "Seat",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_Room",
                table: "Seat",
                column: "room_id",
                principalTable: "Room",
                principalColumn: "room_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seat_Room",
                table: "Seat");

            migrationBuilder.AlterColumn<int>(
                name: "room_id",
                table: "Seat",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_Room",
                table: "Seat",
                column: "room_id",
                principalTable: "Room",
                principalColumn: "room_id");
        }
    }
}
