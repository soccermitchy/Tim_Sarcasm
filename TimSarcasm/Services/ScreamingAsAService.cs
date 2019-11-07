using Discord;
using Discord.Audio;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace TimSarcasm.Services
{
    public class ScreamingAsAService : ServiceEventManager
    {
        private ServerPropertiesService SpService { get; set; }
        private LogService Logger { get; set; }

        private List<ulong> _channelsToScreamIn = new List<ulong>();

        public ScreamingAsAService(DiscordSocketClient client, ServerPropertiesService spService, LogService logger)
        {
            Client = client;
            SpService = spService;
            Logger = logger;
        }
        public void Enable()
        {
            InstallEventListeners(typeof(ScreamingAsAService));
            Logger.Log(new LogMessage(LogSeverity.Info, "SAAS", "ready to start screaming")).Wait();
        }

        [EventListener(Event.MessageReceived)]
        public async Task MessageReceived(SocketMessage message)
        {
            if (!(message.Channel is SocketGuildChannel)) return;
            var guildChannel = (SocketGuildChannel)message.Channel;

            var serverProperties = SpService.GetProperties(guildChannel.Guild.Id);
            // Only enable this feature on servers with temporary voice chats enabled
            if (serverProperties.TempVoiceConfigured)
            {
                var random = new Random();
                if (random.Next(0, 1000) == 1 || message.Content == "test")
                {
                    await Logger.Log(new LogMessage(LogSeverity.Info, "SAAS", "time to start screaming"));
                    var myNickname = guildChannel.Guild.GetUser(Client.CurrentUser.Id).Nickname;
                    if (string.IsNullOrEmpty(myNickname))
                        myNickname = Client.CurrentUser.Username;

                    await StartScreaming(myNickname + "'s Voice Chat", serverProperties.ServerId, serverProperties.TempVoiceCategoryId);
                }
            }
        }

        [EventListener(Event.ChannelCreated)]
        public async Task ChannelCreated(SocketChannel channel)
        {
            if (!(channel is SocketVoiceChannel)) return;
            if (!_channelsToScreamIn.Contains(channel.Id)) return;
            _channelsToScreamIn.Remove(channel.Id);
            var voiceChannel = channel as SocketVoiceChannel;
            Task.Run(() => CreateVc(voiceChannel)).GetAwaiter(); // .GetAwaiter() just to get rid of a warning
        }

        public async Task CreateVc(IVoiceChannel voiceChannel)
        {
            var audioClient = await voiceChannel.ConnectAsync();
            await Logger.Log(new LogMessage(LogSeverity.Info, "SAAS", "created and joined VC"));
            try
            {
                var ffmpeg = Process.Start(new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = $"-hide_banner -loglevel panic -i scream.mp3 -ac 2 -f s16le -ar 48000 pipe:1",
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                });
                await Logger.Log(new LogMessage(LogSeverity.Info, "SAAS", "ffmpeg started"));
                var output = ffmpeg.StandardOutput.BaseStream;
                var discord = audioClient.CreatePCMStream(AudioApplication.Mixed);

                await Logger.Log(new LogMessage(LogSeverity.Info, "SAAS", "sending output"));
                using (audioClient)
                using (ffmpeg)
                using (output)
                using (discord)
                {
                    try
                    {
                        await output.CopyToAsync(discord).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    finally
                    {
                        await discord.FlushAsync().ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task StartScreaming(string vcName, ulong guildId, ulong categoryId)
        {
            var guild = Client.GetGuild(guildId);
            var restVoiceChannel = await guild.CreateVoiceChannelAsync(vcName, (properties) => properties.CategoryId = categoryId);
            _channelsToScreamIn.Add(restVoiceChannel.Id);
        }
    }
}
