using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimSarcasm.Models;
using TimSarcasm.Services;

namespace TimSarcasm.Modules
{
    [Name("ServerProperties"), Summary("Various per-server settings for the bot.")]
    [Group("ServerProperties")]
    [RequireUserPermission(Discord.GuildPermission.Administrator, ErrorMessage = "You must have the Server Administrator permission to use this command.")]
    public class ServerPropertiesModule : ModuleBase<SocketCommandContext>
    {
        private ServerPropertiesService SpService { get; set; }

        public ServerPropertiesModule(ServerPropertiesService spService)
        {
            SpService = spService;
        }
        [Command, Summary("Gets a summary of the server's properties")]
        public async Task PropertySummary()
        {
            var props = SpService.GetProperties(Context.Guild.Id);
            var eb = new EmbedBuilder()
                .WithTitle("Server properties for " + Context.Guild.Name)
                .WithColor(0, 255, 0);
            var logChannel = Context.Guild.GetTextChannel(props.LogChannelId);
            var spamRole = Context.Guild.GetRole(props.SpamRoleId);
            var tempCategory = Context.Guild.GetCategoryChannel(props.TempVoiceCategoryId);
            var tempChannel = Context.Guild.GetVoiceChannel(props.TempVoiceCreateChannelId);

            eb.AddField("Log Channel", logChannel?.Mention ?? "None");
            eb.AddField("Spam Role", spamRole?.Name ?? "None");
            eb.AddField("Temporary Voice Channel Category", tempCategory?.Name ?? "None");
            eb.AddField("Temporary Voice Creation Channel", tempChannel?.Name ?? "None");
            await Context.Channel.SendMessageAsync("", false, eb.Build());
        }

        [Command("LogChannel"),Summary("Gets or sets the channel used for logging.")]
        public async Task LogChannel(SocketTextChannel channel = null)
        {
            var props = SpService.GetProperties(Context.Guild.Id);
            if (channel == null)
            {
                var logChannel = Context.Guild.GetTextChannel(props.LogChannelId);
                if (logChannel == null)
                {
                    await Context.Channel.SendMessageAsync("There is no log channel set.");
                    return;
                }
                await Context.Channel.SendMessageAsync("The log channel is currently " + logChannel.Mention);
                return;
            }
            props.LogChannelId = channel.Id;
            SpService.UpdateProperties(props);
            await Context.Channel.SendMessageAsync("Set the log channel to " + channel.Mention);
        }
        [Command("SpamRole"), Summary("Gets or sets the spam role used for people who spam VC creation")]
        public async Task SpamRole(SocketRole role = null)
        {
            var props = SpService.GetProperties(Context.Guild.Id);
            if (role == null)
            {
                var spamRole = Context.Guild.GetRole(props.SpamRoleId);
                if (spamRole == null)
                {
                    await Context.Channel.SendMessageAsync("There is no spam role set.");
                    return;
                }
                await Context.Channel.SendMessageAsync("The spam role is currently " + spamRole.Name);
                return;
            }
            props.SpamRoleId = role.Id;
            SpService.UpdateProperties(props);
            await Context.Channel.SendMessageAsync("Set the spam role to " + role.Name);
        }

        [Command("TempVoiceCategory"), Summary("Gets or sets the temporary voice channel category")]
        public async Task TempVoiceCategory(SocketCategoryChannel category = null)
        {
            var props = SpService.GetProperties(Context.Guild.Id);
            if (category == null)
            {
                var tempCategory = Context.Guild.GetCategoryChannel(props.TempVoiceCategoryId);
                if (tempCategory == null)
                {
                    await Context.Channel.SendMessageAsync("There is no temporary voice channel category set.");
                    return;
                }
                await Context.Channel.SendMessageAsync("The temporary voice channel category is " + tempCategory.Name);
            }
            props.TempVoiceCategoryId = category.Id;
            SpService.UpdateProperties(props);
            await Context.Channel.SendMessageAsync("Set the temporary voice chat category to " + category.Name);
        }

        [Command("TempVoiceChannel"), Summary("Gets or sets the temporary voice creation channel")]
        public async Task TempVoiceChannel(SocketVoiceChannel channel = null)
        {
            var props = SpService.GetProperties(Context.Guild.Id);
            if (channel == null)
            {
                var voiceChannel = Context.Guild.GetVoiceChannel(props.TempVoiceCreateChannelId);
                if (voiceChannel == null)
                {
                    await Context.Channel.SendMessageAsync("There is no temporary voice creation channel set.");
                    return;
                }
                await Context.Channel.SendMessageAsync("The temporary voice creation channel is currently " + voiceChannel.Name);
                return;
            }
            props.TempVoiceCreateChannelId = channel.Id;
            SpService.UpdateProperties(props);
            await Context.Channel.SendMessageAsync("Set the temporary voice creation channel to " + channel.Name);
        }
    }
}
