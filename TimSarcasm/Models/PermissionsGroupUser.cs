using System;
using System.Collections.Generic;
using System.Text;

namespace TimSarcasm.Models
{
    public class PermissionsGroupUser
    {
        public ulong Id { get; set; }
        public PermissionsGroup Group { get; set; }
        public User User { get; set; }
    }
}
