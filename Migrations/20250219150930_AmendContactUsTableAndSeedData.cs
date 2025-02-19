using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentEnrollmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class AmendContactUsTableAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "ContactUs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Name", "Email", "Password" },
                values: new object[,]
                {
                    { "Admin", "admin@gmail.com", BCrypt.Net.BCrypt.HashPassword("123123") },
                });

            // Insert Students
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "MatricNo", "ICNumber", "Program", "FullName", "Email", "Password", "ContactNumber", "Address1", "PostCode", "City", "State", "Country", "BankAccNum", "BankAccName", "Bank" },
                values: new object[,]
                {
                    { "S12345", "900101-10-1234", "Computer Science", "John Doe", "john.doe@gmail.com", BCrypt.Net.BCrypt.HashPassword("123123"), "0123456789", "123, Main Street", "55520", "Kuala Lumpur", "WP Kuala Lumpur", "Malaysia", "123456789", "John Doe", "Maybank" },
                    { "S67890", "880202-05-5678", "Information Technology", "Jane Smith", "jane.smith@gmail.com", BCrypt.Net.BCrypt.HashPassword("123123"), "0176543210", "456, City Avenue", "56520", "Selangor", "Selangor", "Malaysia", "987654321", "Jane Smith", "CIMB" }
                });

            // Insert SystemSettings
            migrationBuilder.InsertData(
                table: "SystemSettings",
                columns: new[] { "EnabledEnrollment", "CurrentSemester" },
                values: new object[,]
                {
                    { 1, 1 } // Enrollment enabled, current semester = Semester ID 1
                });

            // Insert Semesters
            migrationBuilder.InsertData(
                table: "Semesters",
                columns: new[] { "Name", "StartDate", "EndDate" },
                values: new object[,]
                {
                    { "JAN 2025", new DateTime(2025, 1, 1), new DateTime(2025, 2, 28) },
                    { "MAR 2025", new DateTime(2025, 3, 31), new DateTime(2025, 4, 30) },
                    { "MAY 2025", new DateTime(2025, 5, 1), new DateTime(2026, 7, 31) }
                });

            // Insert Courses
            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseCode", "CourseName", "Day", "StartTime", "EndTime", "Venue", "Amount", "Credits", "Lecturer" },
                values: new object[,]
                {
                    { "MPU123", "Community Service", "Monday", new TimeSpan(20, 0, 0), new TimeSpan(21, 0, 0), "Online", 350.00m, 3, "Dr. Alex Tan" },
                    { "PRG124", "Web Development", "Wednesday", new TimeSpan(20, 0, 0), new TimeSpan(21, 0, 0), "Online", 500.00m, 4, "Prof. Linda Chan" },
                    { "PRG125", "ERP Programming", "Friday", new TimeSpan(20, 0, 0), new TimeSpan(21, 0, 0), "Online", 500.00m, 4, "Dr. Michael Lim" },
                    { "ITM126", "Quantitative Methods", "Thursday", new TimeSpan(20, 30, 30), new TimeSpan(21, 30, 30), "Online", 500.00m, 4, "Dr. Karen Lee" },
                    { "ITM127", "IT Management", "Tuesday", new TimeSpan(20, 30, 30), new TimeSpan(21, 30, 30), "Online", 500.00m, 4, "Prof. David Ng" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "ContactUs");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Students",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
