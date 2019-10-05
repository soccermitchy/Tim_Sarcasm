using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TimSarcasm.Modules
{
    [Name("Help"), Group("help"), Summary("Gets help on commands.")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        public CommandHandler CommandHandler { get; set; }

        [Command]
        [Summary("List modules")]
        public async Task ModuleList()
        {
            var eb = new EmbedBuilder()
                .WithDescription("To get help on a module, do `help module <modulename>`\n")
                .WithTitle("Module List")
                .WithColor(0, 0, 255);
            foreach (var module in CommandHandler.Commands.Modules)
            {
                eb.WithDescription(eb.Description + "\n**" + module.Name + "** - " + (module.Summary ?? "No summary for module."));

            }
            await Context.Channel.SendMessageAsync("", false, eb.Build());
        }
    }
}
