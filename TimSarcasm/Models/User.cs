using System;
using System.Collections.Generic;
using System.Text;

namespace TimSarcasm.Models
{
    public class User
    {
        /// <summary>
        /// Discord user ID
        /// </summary>
        public ulong Id { get; set; }
        public virtual List<PermissionsGroupUser> Groups { get; set; }
    }
}
