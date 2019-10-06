using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TimSarcasm.Services;

namespace TimSarcasm
{
    class Program
    {
        // TODO: Move (most) config values to a database
        public static Configuration Config;
        private DiscordSocketClient _client;


        public Program(string[] args)
        {
            var configPath = args.Length > 0 ? args[1] : "config.json";
            Config = Configuration.FromJson(File.ReadAllText(configPath));
        }
        static void Main(string[] args) => new Program(args).MainAsync().GetAwaiter().GetResult();

        public static Task Log(LogMessage msg)
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
            //_client.UserVoiceStateUpdated += UserVoiceStateUpdated;

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection = ConfigureServices(serviceCollection);
            var services = serviceCollection.BuildServiceProvider();
            await StartServices(services);
            // await services.GetRequiredService<CommandHandler>().InstallCommandsAsync();

            await Task.Delay(-1);
        }

        public IServiceCollection ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(_client);
            serviceCollection.AddSingleton<CommandService>();
            serviceCollection.AddSingleton<CommandHandler>();
            serviceCollection.AddSingleton<TemporaryVoiceChannelService>();
            return serviceCollection;
        }
        public async Task StartServices(IServiceProvider serviceProvider)
        {
            await serviceProvider.GetRequiredService<CommandHandler>().InstallCommandsAsync();
            serviceProvider.GetRequiredService<TemporaryVoiceChannelService>().Enable();
        }
    }
}
