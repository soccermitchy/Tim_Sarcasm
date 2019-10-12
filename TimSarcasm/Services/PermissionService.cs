using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimSarcasm.Models;

namespace TimSarcasm.Services
{
    public class PermissionService
    {
        public DatabaseService DbService { get; set; }
        public PermissionService(DatabaseService databaseService)
        {
            DbService = databaseService;
        }
        public bool? HasPermission(string permission, ulong userId, ulong channelId, ulong serverId)
        {
            var perms = FlattenPermissions(userId, channelId, serverId);
            var matchingPerms = perms.Where(p => p.Permission == permission);
            if (!matchingPerms.Any()) return null;
            return matchingPerms.First().Value;
        }

        public List<PermissionEntry> FlattenPermissions(ulong userId, ulong channelId, ulong serverId)
        {
            var unflattenedGlobalPermissions = DbService.PermissionEntries
                .Where(pe => pe.ParentGroup.Scope == PermissionsScope.Global)
                .Where(pe => pe.ParentGroup.Users.Any(pgu => pgu.User.Id == userId));
            var unflattenedServerPermissions = DbService.PermissionEntries
                .Where(pe => pe.ParentGroup.Scope == PermissionsScope.Server)
                .Where(pe => pe.ParentGroup.ServerId == serverId)
                .Where(pe => pe.ParentGroup.Users.Any(pgu => pgu.User.Id == userId));
            var unflattenedChannelPermissions = DbService.PermissionEntries
                .Where(pe => pe.ParentGroup.Scope == PermissionsScope.Channel)
                .Where(pe => pe.ParentGroup.ServerId == serverId)
                .Where(pe => pe.ParentGroup.ChannelId == channelId)
                .Where(pe => pe.ParentGroup.Users.Any(pgu => pgu.User.Id == userId));

            // Flatten each individual permission set
            var flattenedGlobalPermissions = FlattenPermissionSet(unflattenedGlobalPermissions);
            var flattenedServerPermissions = FlattenPermissionSet(unflattenedServerPermissions);
            var flattenedChannelPermissions = FlattenPermissionSet(unflattenedChannelPermissions);

            // Global permissions have priority over server permissions which have priority over channel permissions.
            var flattenedPermissions = new Dictionary<String, PermissionEntry>();
            foreach (var perm in flattenedChannelPermissions)
            {
                flattenedPermissions.Add(perm.Key, perm.Value);
            }
            foreach (var perm in flattenedServerPermissions)
            {
                if (flattenedPermissions.ContainsKey(perm.Key))
                {
                    flattenedPermissions[perm.Key] = perm.Value;
                    continue;
                }
                flattenedPermissions.Add(perm.Key, perm.Value);
            }
            foreach (var perm in flattenedGlobalPermissions)
            {
                if (flattenedPermissions.ContainsKey(perm.Key))
                {
                    flattenedPermissions[perm.Key] = perm.Value;
                    continue;
                }
                flattenedPermissions.Add(perm.Key, perm.Value);
            }
            return flattenedPermissions.Values.ToList();
        }

        private static Dictionary<string, PermissionEntry> FlattenPermissionSet(IQueryable<PermissionEntry> unflattenedPermissions)
        {
            var flattenedPermissions = new Dictionary<string, PermissionEntry>();
            foreach (var perm in unflattenedPermissions)
            {
                if (flattenedPermissions.ContainsKey(perm.Permission) && flattenedPermissions[perm.Permission].Priority < perm.Priority)
                {
                    flattenedPermissions[perm.Permission] = perm;
                    continue;
                }
                flattenedPermissions.Add(perm.Permission, perm);
            }
            return flattenedPermissions;
        }
    }
}
