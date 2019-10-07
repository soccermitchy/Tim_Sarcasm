using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using TimSarcasm.Models;

namespace TimSarcasm.Services
{
    public class TemporaryVoiceChannelService : ServiceEventManager
    {
        private ServerPropertiesService SpService { get; set; }
        private LogService Logger { get; set; }
        private readonly Dictionary<SocketGuildUser, long> spamProtectionDictionary = new Dictionary<SocketGuildUser, long>();
        private readonly Dictionary<SocketGuildUser, int> spamProtectionCountDictionary = new Dictionary<SocketGuildUser, int>();

        public TemporaryVoiceChannelService(DiscordSocketClient client, ServerPropertiesService spService, LogService logger)
        {
            Client = client;
            SpService = spService;
            Logger = logger;
        }
        public void Enable()
        {
            InstallEventListeners(typeof(TemporaryVoiceChannelService));
        }

        [EventListener(Event.UserVoiceStateUpdated)]
        public async Task UserVoiceStateUpdated(SocketUser user, SocketVoiceState before, SocketVoiceState after)
        {
            SocketGuild guild;
            if (after.VoiceChannel != null)
            {
                guild = after.VoiceChannel.Guild;
            }
            else
            {
                guild = before.VoiceChannel.Guild;
            }
            var serverProperties = SpService.GetProperties(guild.Id);

            var guildUser = guild.GetUser(user.Id);
            if (guildUser == null) return;
            var name = !(string.IsNullOrEmpty(guildUser.Nickname)) ? guildUser.Nickname : user.Username;

            //If the vc they switched to is a "create vc" one
            if (after.VoiceChannel != null && after.VoiceChannel.Id == serverProperties.TempVoiceCreateChannelId)
            {
                if (spamProtectionDictionary.ContainsKey(guildUser))
                {
                    if (spamProtectionCountDictionary[guildUser] > 4)
                    {
                        if (DateTimeOffset.Now.ToUnixTimeSeconds() - spamProtectionDictionary[guildUser] < 60)
                        {
                            await guildUser.AddRoleAsync(guild.GetRole(serverProperties.SpamRoleId));
                            await Logger.Log(new LogMessage(LogSeverity.Warning, "ChannelMaker", "Giving spamrole to " + name + " for spamming VC creation"));
                            var logChannel = Client.GetChannel(serverProperties.LogChannelId) as ITextChannel;
                            await logChannel.SendMessageAsync(guildUser.Mention + " was spamming VC creation, giving spam role.");
                            await RemoveOldVc(before, serverProperties);
                            await guildUser.ModifyAsync(vcUser => { vcUser.Channel = null; });
                            return;
                        }
                        spamProtectionCountDictionary[guildUser] = 0;
                    }
                }

                spamProtectionDictionary[guildUser] = DateTimeOffset.Now.ToUnixTimeSeconds();
                if (!spamProtectionCountDictionary.ContainsKey(guildUser))
                {
                    spamProtectionCountDictionary[guildUser] = 1;
                }
                else
                {
                    spamProtectionCountDictionary[guildUser]++;
                }


                await Logger.Log(new LogMessage(LogSeverity.Info, "ChannelMaker", "Creating VC for " + name));
                var newVoiceChannel = await after.VoiceChannel.Guild.CreateVoiceChannelAsync(name + "'s Voice Chat", (properties) =>
                {
                    properties.CategoryId = serverProperties.TempVoiceCategoryId;
                });
                await guildUser.ModifyAsync(vcUser =>
                {
                    vcUser.Channel = newVoiceChannel;
                });
            }

            await RenameVcIfCreatorLeft(before, serverProperties, user, guildUser);
            await RemoveOldVc(before, serverProperties);
        }

        private async Task RenameVcIfCreatorLeft(SocketVoiceState vc, ServerProperties serverProperties, SocketUser user, IGuildUser guildUser)
        {
            string name = !(string.IsNullOrEmpty(guildUser.Nickname)) ? guildUser.Nickname : user.Username;
            if (vc.VoiceChannel != null &&
                vc.VoiceChannel.Users.Count != 0 &&
                vc.VoiceChannel.CategoryId == serverProperties.TempVoiceCategoryId &&
                vc.VoiceChannel.Id != serverProperties.TempVoiceCreateChannelId &&
                vc.VoiceChannel.Name == (name + "'s Voice Chat"))
            {
                var newOwner = vc.VoiceChannel.Users.First();
                var newName = !(string.IsNullOrEmpty(newOwner.Nickname)) ? newOwner.Nickname : newOwner.Username;
                await vc.VoiceChannel.ModifyAsync(vc => vc.Name = newName + "'s Voice Chat");
            }
        }

        private async Task RemoveOldVc(SocketVoiceState vc, ServerProperties serverProperties)
        {
            if (vc.VoiceChannel != null &&
                vc.VoiceChannel.Users.Count == 0 &&
                vc.VoiceChannel.CategoryId == serverProperties.TempVoiceCategoryId &&
                vc.VoiceChannel.Id != serverProperties.TempVoiceCreateChannelId)
            {
                await vc.VoiceChannel.DeleteAsync();
            }
        }
    }
}
