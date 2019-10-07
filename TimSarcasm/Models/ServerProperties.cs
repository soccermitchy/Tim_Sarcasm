using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TimSarcasm.Models
{
    public class ServerProperties
    {
        [Key]
        public ulong ServerId { get; set; }
        public ulong LogChannelId { get; set; }
        public ulong SpamRoleId { get; set; }
        public ulong TempVoiceCategoryId { get; set; }
        public ulong TempVoiceCreateChannelId { get; set; }
    }
}
