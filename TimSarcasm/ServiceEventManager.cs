using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimSarcasm
{
    public abstract class ServiceEventManager
    {
        public DiscordSocketClient Client;
        public void InstallEventListeners(Type t)
        {
            var methods = t.GetMethods().Where(method => method.GetCustomAttributes(typeof(EventListenerAttribute), false).Length > 0);
            foreach (var method in methods)
            {
                var attrib = method.GetCustomAttributes(typeof(EventListenerAttribute), false).First() as EventListenerAttribute;
                switch (attrib.Event)
                {
                    case Event.UserVoiceStateUpdated:
                        Client.UserVoiceStateUpdated += (user, before, after) => method.Invoke(this, new object[] { user, before, after }) as Task;
                        break;
                    case Event.MessageReceived:
                        Client.MessageReceived += (message) => method.Invoke(this, new object[] { message }) as Task;
                        break;
                    case Event.MessageUpdated:
                        Client.MessageUpdated += (cachedMessage, message, channel) => method.Invoke(this, new object[] { cachedMessage, message, channel }) as Task;
                        break;
                    case Event.ChannelCreated:
                        Client.ChannelCreated += (channel) => method.Invoke(this, new object[] { channel }) as Task;
                        break;
                }
            }
        }
    }
}
