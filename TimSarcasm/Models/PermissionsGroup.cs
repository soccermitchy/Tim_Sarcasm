using System;
using System.Collections.Generic;
using System.Text;

namespace TimSarcasm.Models
{
    public class PermissionsGroup
    {
        /// <summary>
        /// Internal ID of the permissions group
        /// </summary>
        public ulong Id { get; set; }
        /// <summary>
        /// Name of the permissions group
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// Scope this permissions group applies to.
        /// </summary>
        public PermissionsScope Scope { get; set; }
        /// <summary>
        /// ID of the server this permissions group belongs to. 0 if scope is global.
        /// </summary>
        public ulong ServerId { get; set; }
        /// <summary>
        /// ID of the channel this permissions group belongs to. 0 if scope is global or server only
        /// </summary>
        public ulong ChannelId { get; set; }
        /// <summary>
        /// Users in this group
        /// </summary>
        public List<PermissionsGroupUser> Users { get; set; }
        /// <summary>
        /// Permissions in this group
        /// </summary>
        public List<PermissionEntry> Permissions { get; set; }
    }
}
