using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TimSarcasm.Modules
{

    public class EmojiCommandsModule : ModuleBase<SocketCommandContext>
    {
        [Command("b")]
        [Summary("b")]
        public async Task B()
        {
            await Context.Message.Channel.SendMessageAsync("🅱");
        }
    }
}
