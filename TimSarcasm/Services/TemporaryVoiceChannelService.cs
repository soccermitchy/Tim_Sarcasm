using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TimSarcasm.Services
{
    public class TemporaryVoiceChannelService : ServiceEventManager
    {
        private Dictionary<SocketGuildUser, long> spamProtectionDictionary = new Dictionary<SocketGuildUser, long>();
        private Dictionary<SocketGuildUser, int> spamProtectionCountDictionary = new Dictionary<SocketGuildUser, int>();
        public TemporaryVoiceChannelService(DiscordSocketClient client)
        {
            Client = client;
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

            var guildUser = guild.GetUser(user.Id);
            if (guildUser == null) return;
            var name = !(string.IsNullOrEmpty(guildUser.Nickname)) ? guildUser.Nickname : user.Username;

            if (after.VoiceChannel != null && after.VoiceChannel.Id == Program.Config.CreateVoiceChannelId)
            {
                if (spamProtectionDictionary.ContainsKey(guildUser))
                {
                    if (spamProtectionCountDictionary[guildUser] > 4)
                    {
                        if (DateTimeOffset.Now.ToUnixTimeSeconds() - spamProtectionDictionary[guildUser] < 60)
                        {
                            await guildUser.AddRoleAsync(guild.GetRole(Program.Config.SpamRoleId));
                            await Program.Log(new LogMessage(LogSeverity.Warning, "ChannelMaker", "Giving spamrole to " + name + " for spamming VC creation"));
                            var logChannel = Client.GetChannel(Program.Config.ModLogChannelId) as ITextChannel;
                            await logChannel.SendMessageAsync(guildUser.Mention + " was spamming VC creation, giving spam role.");
                            await RemoveOldVc(before);
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


                await Program.Log(new LogMessage(LogSeverity.Info, "ChannelMaker", "Creating VC for " + name));
                var newVoiceChannel = await after.VoiceChannel.Guild.CreateVoiceChannelAsync(name + "'s Voice Chat", (properties) =>
                {
                    properties.CategoryId = Program.Config.VoiceChannelCategory;
                });
                await guildUser.ModifyAsync(vcUser =>
                {
                    vcUser.Channel = newVoiceChannel;
                });
            }

            await RemoveOldVc(before);
        }

        private async Task RemoveOldVc(SocketVoiceState before)
        {
            if (before.VoiceChannel != null &&
                before.VoiceChannel.Users.Count == 0 &&
                before.VoiceChannel.CategoryId == Program.Config.VoiceChannelCategory &&
                before.VoiceChannel.Id != Program.Config.CreateVoiceChannelId)
            {
                await before.VoiceChannel.DeleteAsync();
            }
        }
    }
}
