using Microsoft.EntityFrameworkCore.Migrations;

namespace TimSarcasm.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServerProperties",
                columns: table => new
                {
                    ServerId = table.Column<uint>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LogChannelId = table.Column<uint>(nullable: false),
                    SpamRoleId = table.Column<uint>(nullable: false),
                    TempVoiceCategoryId = table.Column<uint>(nullable: false),
                    TempVoiceCreateChannelId = table.Column<uint>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerProperties", x => x.ServerId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServerProperties");
        }
    }
}
