using Microsoft.EntityFrameworkCore.Migrations;

namespace TimSarcasm.Migrations
{
    public partial class MessageLoggingAttachments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoggedMessageAttachments",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MessageId = table.Column<ulong>(nullable: false),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoggedMessageAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoggedMessageAttachments_LoggedMessages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "LoggedMessages",
                        principalColumn: "MessageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoggedMessageAttachments_MessageId",
                table: "LoggedMessageAttachments",
                column: "MessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoggedMessageAttachments");
        }
    }
}
