using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentService.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    AppointmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.AppointmentID);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentHistory",
                columns: table => new
                {
                    AppointmentHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    DoctorID = table.Column<int>(type: "int", nullable: false),
                    DateSelected = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeSelected = table.Column<TimeOnly>(type: "time", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentHistory", x => x.AppointmentHistoryID);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentStatus",
                columns: table => new
                {
                    AppointmentStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentStatusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentStatus", x => x.AppointmentStatusID);
                });

            migrationBuilder.CreateTable(
                name: "Doctor",
                columns: table => new
                {
                    DoctorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctor", x => x.DoctorID);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    PatientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.PatientID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "AppointmentHistory");

            migrationBuilder.DropTable(
                name: "AppointmentStatus");

            migrationBuilder.DropTable(
                name: "Doctor");

            migrationBuilder.DropTable(
                name: "Patient");
        }
    }
}
