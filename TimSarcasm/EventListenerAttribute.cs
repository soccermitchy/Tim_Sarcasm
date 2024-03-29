﻿using System;

namespace TimSarcasm
{
    public enum Event
    {
        UserVoiceStateUpdated,
        MessageReceived,
        MessageUpdated
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class EventListenerAttribute : Attribute
    {
        public Event Event { get; private set; }
        public EventListenerAttribute(Event ev) {
            Event = ev;
        }
    }
}