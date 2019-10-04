using Discord;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace VoiceManager
{
    class Program
    {
        public static Configuration Config;
        private DiscordSocketClient _client;
        public Program(string[] args)
        {
            var configPath = args.Length > 0 ? args[1] : "config.json";
            Config = Configuration.FromJson(File.ReadAllText(configPath));
        }
        static void Main(string[] args) => new Program(args).MainAsync().GetAwaiter().GetResult();

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;
            await _client.LoginAsync(TokenType.Bot, Config.Token);
            await _client.StartAsync();
            _client.UserVoiceStateUpdated += UserVoiceStateUpdated;
            var handler = new CommandHandler(_client, new Discord.Commands.CommandService());
            await handler.InstallCommandsAsync();
            await Task.Delay(-1);   
        }



        private async Task UserVoiceStateUpdated(SocketUser user, SocketVoiceState before, SocketVoiceState after)
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

            if (after.VoiceChannel != null && after.VoiceChannel.Id == Config.CreateVoiceChannelId)
            {
                await Log(new LogMessage(LogSeverity.Info, "ChannelMaker", "Creating VC for " + name));
                var newVoiceChannel = await after.VoiceChannel.Guild.CreateVoiceChannelAsync(name + "'s Voice Chat", (properties) =>
                {
                    properties.CategoryId = Config.VoiceChannelCategory;
                });
                await guildUser.ModifyAsync(vcUser =>
                {
                    vcUser.Channel = newVoiceChannel;
                });
            }

            if (before.VoiceChannel != null && 
                before.VoiceChannel.Users.Count == 0 && 
                before.VoiceChannel.CategoryId == Config.VoiceChannelCategory && 
                before.VoiceChannel.Id != Config.CreateVoiceChannelId)
            {
                await before.VoiceChannel.DeleteAsync();
            }
        }
    }
}
