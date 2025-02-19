using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentEnrollmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class AmendTeachingEvaluationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeachingEvaluations_Courses_CourseID",
                table: "TeachingEvaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_TeachingEvaluations_Students_StudentID",
                table: "TeachingEvaluations");

            migrationBuilder.DropIndex(
                name: "IX_TeachingEvaluations_CourseID",
                table: "TeachingEvaluations");

            migrationBuilder.DropColumn(
                name: "CourseID",
                table: "TeachingEvaluations");

            migrationBuilder.DropColumn(
                name: "Faculty",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                table: "TeachingEvaluations",
                newName: "EnrollmentID");

            migrationBuilder.RenameIndex(
                name: "IX_TeachingEvaluations_StudentID",
                table: "TeachingEvaluations",
                newName: "IX_TeachingEvaluations_EnrollmentID");

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "TeachingEvaluations",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "TeachingEvaluations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_TeachingEvaluations_Enrollments_EnrollmentID",
                table: "TeachingEvaluations",
                column: "EnrollmentID",
                principalTable: "Enrollments",
                principalColumn: "EnrollmentID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeachingEvaluations_Enrollments_EnrollmentID",
                table: "TeachingEvaluations");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "TeachingEvaluations");

            migrationBuilder.RenameColumn(
                name: "EnrollmentID",
                table: "TeachingEvaluations",
                newName: "StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_TeachingEvaluations_EnrollmentID",
                table: "TeachingEvaluations",
                newName: "IX_TeachingEvaluations_StudentID");

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "TeachingEvaluations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<int>(
                name: "CourseID",
                table: "TeachingEvaluations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Faculty",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TeachingEvaluations_CourseID",
                table: "TeachingEvaluations",
                column: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_TeachingEvaluations_Courses_CourseID",
                table: "TeachingEvaluations",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeachingEvaluations_Students_StudentID",
                table: "TeachingEvaluations",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "StudentID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
