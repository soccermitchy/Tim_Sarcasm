using System;
using System.Collections.Generic;
using System.Text;

namespace TimSarcasm.Models
{
    public class PermissionEntry
    {
        public ulong Id { get; set; }
        public string Permission { get; set; }
        public virtual PermissionsGroup ParentGroup { get; set; }
        public int Priority { get; set; }
        public bool Value { get; set; }
    }
}
