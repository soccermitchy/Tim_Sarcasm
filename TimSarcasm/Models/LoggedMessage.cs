using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TimSarcasm.Models
{
    public class LoggedMessage
    {
        [Key]
        public ulong MessageId { get; set; }
        public ulong ServerId { get; set; }
        public ulong ChannelId { get; set; }
        public ulong AuthorId { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime EditTimestamp { get; set; }

        public List<LoggedMessageAttachment> Attachments { get; set; }
    }
}
