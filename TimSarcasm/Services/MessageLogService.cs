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
            LogAttachments(message, loggedMessage);

            if (message.Channel is SocketGuildChannel)
                loggedMessage.ServerId = (message.Channel as SocketGuildChannel).Guild.Id;
            DbService.LoggedMessages.Add(loggedMessage);
            await DbService.SaveChangesAsync();
        }

        private void LogAttachments(SocketMessage message, LoggedMessage loggedMessage)
        {
            if (message.Attachments.Any())
            {
                foreach (var attachment in message.Attachments)
                {
                    if (DbService.LoggedMessageAttachments.Any(att => att.Id == attachment.Id)) continue;
                    var loggedAttachment = new LoggedMessageAttachment()
                    {
                        Id = attachment.Id,
                        MessageId = loggedMessage.MessageId,
                        Message = loggedMessage,
                        Url = attachment.Url
                    };
                    DbService.LoggedMessageAttachments.Add(loggedAttachment);
                }
            }
        }

        [EventListener(Event.MessageUpdated)]
        public async Task MessageUpdated(Cacheable<IMessage, ulong> cachableMessage, SocketMessage message, ISocketMessageChannel channel)
        {
            var loggedMessages = DbService.LoggedMessages.Where(msg => msg.MessageId == message.Id);
            if (loggedMessages.Count() == 0) return;
            var loggedMessage = loggedMessages.First();
            loggedMessage.Message = message.Content;
            loggedMessage.EditTimestamp = message.EditedTimestamp.HasValue ? message.EditedTimestamp.Value.DateTime : DateTime.Now;
            LogAttachments(message, loggedMessage);
            DbService.LoggedMessages.Update(loggedMessage);
            await DbService.SaveChangesAsync();
        }
    }
}
