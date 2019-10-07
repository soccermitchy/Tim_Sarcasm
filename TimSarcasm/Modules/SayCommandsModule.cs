using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace TimSarcasm.Modules
{
    [Name("Say"), Summary("Talking in chat commands.")]
    public class SayCommandsModule : ModuleBase<SocketCommandContext>
    {
        [Command("say"), Summary("Say a message in chat"), RequireRole("tim")]
        public async Task Say(params string[] message)
        {
            await Context.Message.DeleteAsync();
            await Context.Message.Channel.SendMessageAsync(message.Aggregate((a, b) => a + " " + b));
        }
    }
}
