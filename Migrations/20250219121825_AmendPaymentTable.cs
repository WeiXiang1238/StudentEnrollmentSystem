using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentEnrollmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class AmendPaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "Enrollments",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "PaymentID",
                table: "Enrollments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_PaymentID",
                table: "Enrollments",
                column: "PaymentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Payments_PaymentID",
                table: "Enrollments",
                column: "PaymentID",
                principalTable: "Payments",
                principalColumn: "PaymentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Payments_PaymentID",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_PaymentID",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "PaymentID",
                table: "Enrollments");

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "Enrollments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceID);
                    table.ForeignKey(
                        name: "FK_Invoices_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_StudentID",
                table: "Invoices",
                column: "StudentID");
        }
    }
}
