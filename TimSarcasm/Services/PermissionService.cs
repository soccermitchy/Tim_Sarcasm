using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimSarcasm.Models;

namespace TimSarcasm.Services
{
    public class PermissionService
    {
        public DatabaseService DbService { get; set; }
        public DiscordSocketClient Client { get; set; }
        public PermissionService(DatabaseService databaseService, DiscordSocketClient client) 
        {
            DbService = databaseService;
            Client = client;
        }
        public async Task<bool?> HasPermission(string permission, ulong userId, ulong channelId, ulong serverId)
        {
            if ((await Client.GetApplicationInfoAsync().ConfigureAwait(false)).Owner.Id == userId) return true;
            var perms = FlattenPermissions(userId, channelId, serverId);
            var matchingPerms = perms.Where(p => p.Permission == permission);
            if (!matchingPerms.Any()) return null;
            return matchingPerms.First().Value;
        }
        public async Task<bool?> HasPermission(string permission, ICommandContext context)
        {
            return await HasPermission(permission, context.User.Id, context.Channel.Id, context.Guild.Id).ConfigureAwait(false);
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

        public PermissionsGroup GetGroup(PermissionsScope scope, string name, ulong ServerId = 0, ulong ChannelId = 0)
        {
            IQueryable<PermissionsGroup> results;
            switch (scope)
            {
                case PermissionsScope.Global:
                    results = DbService.PermissionGroups
                        .Where(pg => pg.GroupName == name)
                        .Where(pg => pg.Scope == PermissionsScope.Global);
                    if (!results.Any()) return null;
                    return results.First();

                case PermissionsScope.Server:
                    results = DbService.PermissionGroups
                        .Where(pg => pg.GroupName == name)
                        .Where(pg => pg.Scope == PermissionsScope.Server)
                        .Where(pg => pg.ServerId == ServerId);
                    if (!results.Any()) return null;
                    return results.First();

                case PermissionsScope.Channel:
                    results = DbService.PermissionGroups
                        .Where(pg => pg.GroupName == name)
                        .Where(pg => pg.Scope == PermissionsScope.Channel)
                        .Where(pg => pg.ServerId == ServerId)
                        .Where(pg => pg.ChannelId == ChannelId);
                    if (!results.Any()) return null;
                    return results.First();
            }
            return null;
        }

        public async Task<PermissionsGroup> CreateGroup(PermissionsScope scope, string name, ulong ServerId = 0, ulong ChannelId = 0)
        {
            var group = new PermissionsGroup()
            {
                GroupName = name,
                Scope = scope,
                ServerId = ServerId,
                ChannelId = ChannelId
            };
            group = DbService.PermissionGroups.Add(group).Entity;
            await DbService.SaveChangesAsync().ConfigureAwait(false);
            return group;
        }

        public async Task DeleteGroup(PermissionsGroup group)
        {
            DbService.PermissionGroups.Remove(group);
            await DbService.SaveChangesAsync().ConfigureAwait(false);
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
