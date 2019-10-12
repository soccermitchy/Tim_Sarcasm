using System;
using System.Collections.Generic;
using System.Text;

namespace TimSarcasm.Models
{
    public class PermissionsGroupUser
    {
        public ulong Id { get; set; }
        public virtual PermissionsGroup Group { get; set; }
        public virtual User User { get; set; }
    }
}
