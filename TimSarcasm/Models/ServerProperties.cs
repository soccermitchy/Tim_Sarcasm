using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TimSarcasm.Models
{
    public class ServerProperties
    {
        [Key]
        public uint ServerId { get; set; }
        public uint LogChannelId { get; set; }
        public uint SpamRoleId { get; set; }
        public uint TempVoiceCategoryId { get; set; }
        public uint TempVoiceCreateChannelId { get; set; }
    }
}
