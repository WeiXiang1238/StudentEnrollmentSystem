using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentEnrollmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class AmendStudentTableAndRemoveTimeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Timetables");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Students");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Students",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "Students",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Students",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactName",
                table: "Students",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactPhoneNumber",
                table: "Students",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactRelationship",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ICNumber",
                table: "Students",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MatricNo",
                table: "Students",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Students",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostCode",
                table: "Students",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Program",
                table: "Students",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Enrollments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address1",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EmergencyContactName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EmergencyContactPhoneNumber",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EmergencyContactRelationship",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ICNumber",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "MatricNo",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "PostCode",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Program",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "Enrollments");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Timetables",
                columns: table => new
                {
                    TimetableID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseID = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Venue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timetables", x => x.TimetableID);
                    table.ForeignKey(
                        name: "FK_Timetables_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_CourseID",
                table: "Timetables",
                column: "CourseID");
        }
    }
}
