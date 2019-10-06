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
        static void Main() => new Program().MainAsync().GetAwaiter().GetResult();

        public static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public async Task MainAsync()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection = ConfigureServices(serviceCollection);
            var services = serviceCollection.BuildServiceProvider();
            await StartServices(services);
            // await services.GetRequiredService<CommandHandler>().InstallCommandsAsync();

            await Task.Delay(-1);
        }

        public IServiceCollection ConfigureServices(IServiceCollection serviceCollection)
        {
            //serviceCollection.AddSingleton(_client);
            serviceCollection.AddSingleton<DiscordSocketClient>();
            // TODO: maybe have this somehow automatically:tm: bind to w/e class needs it to show where logs are from
            serviceCollection.AddSingleton<LogService>();
            serviceCollection.AddSingleton<ConfigurationService>();
            serviceCollection.AddSingleton<CommandService>();
            serviceCollection.AddSingleton<CommandHandler>();
            serviceCollection.AddSingleton<TemporaryVoiceChannelService>();
            return serviceCollection;
        }
        public async Task StartServices(IServiceProvider serviceProvider)
        {
            var config = serviceProvider.GetRequiredService<ConfigurationService>().Config;
            var logger = serviceProvider.GetRequiredService<LogService>();

            // Discord client init
            var client = serviceProvider.GetRequiredService<DiscordSocketClient>();
            client.Log += logger.Log;
            await client.LoginAsync(TokenType.Bot, config.Token);
            await client.StartAsync();

            await serviceProvider.GetRequiredService<CommandHandler>().InstallCommandsAsync();
            serviceProvider.GetRequiredService<TemporaryVoiceChannelService>().Enable();
        }
    }
}
