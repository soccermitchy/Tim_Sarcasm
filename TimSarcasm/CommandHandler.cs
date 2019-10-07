using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TimSarcasm
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        public readonly CommandService Commands;
        private IServiceProvider Services;
        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services)
        {
            Commands = commands;
            _client = client;
            Services = services;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandsAsync;
            await Commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: Services);
        }
        private async Task HandleCommandsAsync(SocketMessage messageParam)
        {
            if (!(messageParam is SocketUserMessage message)) return;
            int argPos = 0;
            if (!(
                message.HasMentionPrefix(_client.CurrentUser, ref argPos) ||
                message.HasCharPrefix('~', ref argPos)) ||
                message.Author.IsBot)
                return;
            var context = new SocketCommandContext(_client, message);
            var result = await Commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: Services);
            if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                await message.Channel.SendMessageAsync("Error: " + result.ErrorReason);
            //if (!result.IsSuccess)
            //    await message.Channel.SendMessageAsync("Error: " + result.ErrorReason);
        }
    }
}
