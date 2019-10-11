using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimSarcasm.Migrations
{
    public partial class MessageLoggingTimestamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EditTimestamp",
                table: "LoggedMessages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "LoggedMessages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditTimestamp",
                table: "LoggedMessages");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "LoggedMessages");
        }
    }
}
