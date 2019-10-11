using Microsoft.EntityFrameworkCore.Migrations;

namespace TimSarcasm.Migrations
{
    public partial class MessageLogging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoggedMessages",
                columns: table => new
                {
                    MessageId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServerId = table.Column<ulong>(nullable: false),
                    ChannelId = table.Column<ulong>(nullable: false),
                    AuthorId = table.Column<ulong>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoggedMessages", x => x.MessageId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoggedMessages");
        }
    }
}
