using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimSarcasm.Services;
using Discord;
using Discord.WebSocket;
using System.Threading;
using System.Linq;

namespace TimSarcasm.Modules
{
    [Name("Admin"),Summary("Admin commands")]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        public ServerPropertiesService SpService { get; set; }

        [Command("purge"), Summary("Purge messages from a channel")]
        [RequireUserPermission(Discord.GuildPermission.ManageMessages)]
        public async Task Purge(int amount)
        {
            var messages = (await Context.Channel.GetMessagesAsync(amount+1).FlattenAsync()).Where(msg => msg.IsPinned == false);
            await (Context.Channel as ITextChannel).DeleteMessagesAsync(messages);
            var logChannel = Context.Guild.GetTextChannel(SpService.GetProperties(Context.Guild.Id).LogChannelId);
            if (logChannel != null)
                await logChannel.SendMessageAsync(Context.Message.Author.Mention + " has just purged " + messages.Count() + " messages from " + (Context.Channel as SocketTextChannel).Mention);
            await Context.Channel.SendMessageAsync("Done.");
        }
    }
}
