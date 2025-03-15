using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedSeatTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seat_SeatType",
                table: "Seat");

            migrationBuilder.AlterColumn<int>(
                name: "type_id",
                table: "Seat",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_SeatType",
                table: "Seat",
                column: "type_id",
                principalTable: "SeatType",
                principalColumn: "type_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seat_SeatType",
                table: "Seat");

            migrationBuilder.AlterColumn<int>(
                name: "type_id",
                table: "Seat",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_SeatType",
                table: "Seat",
                column: "type_id",
                principalTable: "SeatType",
                principalColumn: "type_id");
        }
    }
}
