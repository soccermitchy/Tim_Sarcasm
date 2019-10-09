using System;
using System.Collections.Generic;
using System.Text;

namespace TimSarcasm.Models
{
    public class PermissionEntry
    {
        public ulong Id { get; set; }
        public string Permission { get; set; }
        public PermissionsGroup ParentGroup { get; set; }
    }
}
