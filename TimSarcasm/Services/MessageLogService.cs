using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimSarcasm.Models;

namespace TimSarcasm.Services
{
    public class MessageLogService : ServiceEventManager
    {
        public DatabaseService DbService { get; }
        public MessageLogService(DiscordSocketClient client, DatabaseService dbService)
        {
            Client = client;
            DbService = dbService;
        }

        public void Enable()
        {
            InstallEventListeners(typeof(MessageLogService));
        }

        [EventListener(Event.MessageReceived)]
        public async Task MessageReceived(SocketMessage message)
        {
            var loggedMessage = new LoggedMessage()
            {
                MessageId = message.Id,
                AuthorId = message.Author.Id,
                ChannelId = message.Channel.Id,
                Message = message.Content,
                Timestamp = message.Timestamp.DateTime
            };
            if (message.Channel is SocketGuildChannel)
                loggedMessage.ServerId = (message.Channel as SocketGuildChannel).Guild.Id;
            DbService.LoggedMessages.Add(loggedMessage);
            await DbService.SaveChangesAsync();
        }

        [EventListener(Event.MessageUpdated)]
        public async Task MessageUpdated(Cacheable<IMessage, ulong> cachableMessage, SocketMessage message, ISocketMessageChannel channel)
        {
            var loggedMessages = DbService.LoggedMessages.Where(msg => msg.MessageId == message.Id);
            if (loggedMessages.Count() == 0) return;
            var loggedMessage = loggedMessages.First();
            loggedMessage.Message = message.Content;
            loggedMessage.EditTimestamp = message.EditedTimestamp.HasValue ? message.EditedTimestamp.Value.DateTime : DateTime.Now;
            DbService.LoggedMessages.Update(loggedMessage);
            await DbService.SaveChangesAsync();
        }
    }
}
