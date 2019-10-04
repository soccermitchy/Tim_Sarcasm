using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VoiceManager
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            _commands = commands;
            _client = client;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandsAsync;
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
            foreach (var module in _commands.Modules)
            {
                Console.WriteLine("Found module: " + module.Name);
                foreach (var cmd in module.Commands)
                {
                    Console.WriteLine("Command in " + module.Name + ": " + cmd.Name);
                }
            }
        }
        private async Task HandleCommandsAsync(SocketMessage messageParam)
        {
            if (!(messageParam is SocketUserMessage message)) return;
            int argPos = 0;
            if (!message.HasCharPrefix('~', ref argPos))
                return;
            Console.WriteLine("argument pos: " + argPos);
            var context = new SocketCommandContext(_client, message);
            var result = await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null);
            if (!result.IsSuccess)
                Console.WriteLine(result.ErrorReason + " - " + context.Message.Content);
        }
    }
}
