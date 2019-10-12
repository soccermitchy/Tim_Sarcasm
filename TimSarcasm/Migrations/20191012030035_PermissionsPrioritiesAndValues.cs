using Microsoft.EntityFrameworkCore.Migrations;

namespace TimSarcasm.Migrations
{
    public partial class PermissionsPrioritiesAndValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "PermissionEntries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Value",
                table: "PermissionEntries",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "PermissionEntries");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "PermissionEntries");
        }
    }
}
