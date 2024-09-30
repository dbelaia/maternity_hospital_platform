using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentService.Migrations
{
    /// <inheritdoc />
    public partial class FixDataTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeSelected",
                table: "AppointmentHistory");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateSelected",
                table: "AppointmentHistory",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateSelected",
                table: "AppointmentHistory",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "TimeSelected",
                table: "AppointmentHistory",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }
    }
}
