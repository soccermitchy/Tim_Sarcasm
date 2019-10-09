using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TimSarcasm.Modules
{
    [Name("badmeme"),Summary("your meme is bad")]
    public class BadMemeModule : ModuleBase<SocketCommandContext>
    {
        [Command("badmeme"), Summary("your meme is bad")]
        public async Task BadMeme()
        {
            await Context.Message.DeleteAsync();
            await Context.Channel.SendMessageAsync("https://cdn.discordapp.com/attachments/586803292072443963/609994677944451082/image0-1.gif");
        }
    }
}
