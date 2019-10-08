using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimSarcasm.Services;
using Discord;
using Discord.WebSocket;
using System.Threading;

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
            var messages = await Context.Channel.GetMessagesAsync(amount+1).FlattenAsync();

            var messageNum = 0;
            foreach (var message in messages)
            {
                messageNum++;
                if (!message.IsPinned)
                    await message.DeleteAsync();
                if (messageNum == 10)
                {
                    messageNum = 0;
                    Thread.Sleep(1000);
                }
            }
            var logChannel = Context.Guild.GetTextChannel(SpService.GetProperties(Context.Guild.Id).LogChannelId);
            if (logChannel != null)
                await logChannel.SendMessageAsync(Context.Message.Author.Mention + " has just purged " + amount + " messages from " + (Context.Channel as SocketTextChannel).Mention);
            await Context.Channel.SendMessageAsync("Done.");
        }   
    }
}
