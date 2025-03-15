using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedRoomTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "seats_no",
                table: "Room",
                newName: "total_row");

            migrationBuilder.AddColumn<int>(
                name: "total_column",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "room_id",
                keyValue: 1,
                columns: new[] { "total_column", "total_row" },
                values: new object[] { 10, 5 });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "room_id",
                keyValue: 2,
                columns: new[] { "total_column", "total_row" },
                values: new object[] { 10, 8 });

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "room_id",
                keyValue: 3,
                columns: new[] { "total_column", "total_row" },
                values: new object[] { 10, 12 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_column",
                table: "Room");

            migrationBuilder.RenameColumn(
                name: "total_row",
                table: "Room",
                newName: "seats_no");

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "room_id",
                keyValue: 1,
                column: "seats_no",
                value: 100);

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "room_id",
                keyValue: 2,
                column: "seats_no",
                value: 80);

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "room_id",
                keyValue: 3,
                column: "seats_no",
                value: 120);
        }
    }
}
