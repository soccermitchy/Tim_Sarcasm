using System;
using System.Collections.Generic;
using System.Text;

namespace TimSarcasm.Models
{
    public class LoggedMessageAttachment
    {
        public ulong Id { get; set; }

        public ulong MessageId { get; set; }
        public virtual LoggedMessage Message { get; set; }

        public string Url { get; set; }
    }
}
