using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentEnrollmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class AmendEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paid",
                table: "Enrollments");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Enrollments",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Enrollments");

            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "Enrollments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
