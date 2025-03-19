using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentTableToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Booking_BookingId",
                table: "Payment");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "Payment",
                newName: "session_id");

            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "Payment",
                newName: "payment_status");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "Payment",
                newName: "payment_method");

            migrationBuilder.RenameColumn(
                name: "PaymentDueDate",
                table: "Payment",
                newName: "payment_due_date");

            migrationBuilder.RenameColumn(
                name: "PaymentDate",
                table: "Payment",
                newName: "payment_date");

            migrationBuilder.RenameColumn(
                name: "PamentIntentId",
                table: "Payment",
                newName: "payment_intent_id");

            migrationBuilder.RenameColumn(
                name: "BookingId",
                table: "Payment",
                newName: "booking_id");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "Payment",
                newName: "payment_id");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_BookingId",
                table: "Payment",
                newName: "IX_Payment_booking_id");

            migrationBuilder.AlterColumn<string>(
                name: "payment_status",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "pending",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "payment_method",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "cash",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Booking",
                table: "Payment",
                column: "booking_id",
                principalTable: "Booking",
                principalColumn: "booking_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Booking",
                table: "Payment");

            migrationBuilder.RenameColumn(
                name: "session_id",
                table: "Payment",
                newName: "SessionId");

            migrationBuilder.RenameColumn(
                name: "payment_status",
                table: "Payment",
                newName: "PaymentStatus");

            migrationBuilder.RenameColumn(
                name: "payment_method",
                table: "Payment",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "payment_intent_id",
                table: "Payment",
                newName: "PamentIntentId");

            migrationBuilder.RenameColumn(
                name: "payment_due_date",
                table: "Payment",
                newName: "PaymentDueDate");

            migrationBuilder.RenameColumn(
                name: "payment_date",
                table: "Payment",
                newName: "PaymentDate");

            migrationBuilder.RenameColumn(
                name: "booking_id",
                table: "Payment",
                newName: "BookingId");

            migrationBuilder.RenameColumn(
                name: "payment_id",
                table: "Payment",
                newName: "PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_booking_id",
                table: "Payment",
                newName: "IX_Payment_BookingId");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentStatus",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "pending");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "cash");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Booking_BookingId",
                table: "Payment",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "booking_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
