using Discord.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimSarcasm.Services;
namespace TimSarcasm.Modules
{
    [RequireOwner]
    [Name("Database"), Summary("Database maintainance commands.")]
    [Group("database")]
    public class DatabaseModule : ModuleBase<SocketCommandContext>
    {
        private DatabaseService DbService { get; set; }
        public DatabaseModule(DatabaseService dbService)
        {
            DbService = dbService;
        }

        [Command("migrate"), Summary("Run database migrations")]
        public async Task Migrate()
        {
            DbService.Database.Migrate();
            await Context.Channel.SendMessageAsync("Done");
        }

        [Command("version"), Summary("See the current version of the database")]
        public async Task Version()
        {
            var version = DbService.Database.GetAppliedMigrations().Last();
            await Context.Channel.SendMessageAsync("Last migration was " + version);
        }
    }
}
