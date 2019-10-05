using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TimSarcasm.Modules
{
    [Name("Poll"), Summary("Creates polls.")]
    public class PollModule : ModuleBase<SocketCommandContext>
    {
        [Command("poll")]
        [Summary("Creates a new poll")]
        public async Task Poll(params string[] words)
        {
            await Context.Message.DeleteAsync();
            var question = await Context.Message.Channel.SendMessageAsync("Poll from " + Context.Message.Author.Mention + ": " + String.Join(' ', words));
            await question.AddReactionsAsync(new[] { new Emoji("👍"), new Emoji("😐"), new Emoji("👎") });
        }
    }
}
