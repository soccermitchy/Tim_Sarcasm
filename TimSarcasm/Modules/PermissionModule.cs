using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimSarcasm.Models;
using TimSarcasm.Services;
using TimSarcasm.Util;

namespace TimSarcasm.Modules
{
    [Name("Permissions"),Group("Permission"),Summary("Permissions management commands")]
    [Remarks("Global groups start with %, server groups start with !, and channel groups start with ^")]
    public class PermissionModule : ModuleBase<SocketCommandContext>
    {
        private PermissionService PermissionService { get; }

        public PermissionModule(PermissionService ps)
        {
            this.PermissionService = ps;
        }

        [Command("HasPermission"),Summary("Check if a user has a permission in a given context")]

        public async Task HasPermission(SocketUser user, SocketTextChannel channel, string permission)
        {
            var result = await PermissionService.HasPermission(permission, user.Id, channel.Id, Context.Guild.Id);
            var reply = "Permission check returned **";
            reply += (result.HasValue ? (result.Value ? "true" : "false") : "undefined");
            reply += "**\n\n";
            reply += "Context:\n";
            reply += "Guild = " + Context.Guild.Name + " (" + Context.Guild.Id + ")\n";
            reply += "Channel = " + channel.Mention + " (" + channel.Id + ")\n";
            reply += "User = " + user.Mention + " (" + user.Id + ")";

            await Context.Channel.SendMessageAsync(reply);
        }

        [Command("CreateGroup"),Summary("Creates a new group")]
        public async Task CreateGroup(PermissionsGroup group)
        {
            var perm = group.Scope switch
            {
                PermissionsScope.Global => "permissions.creategroup.global",
                PermissionsScope.Server => "permissions.creategroup.server",
                PermissionsScope.Channel => "permissions.creategroup.channel",
                _ => "permissions.creategroup.any",
            };
            var permsCheck = await PermissionService.HasPermission(perm, Context).ConfigureAwait(false);
            if (!permsCheck.HasValue || !permsCheck.Value)
            {
                await Context.Channel.SendMessageAsync("You do not have permission to do that!").ConfigureAwait(false);
                return;
            }
            if (group.Id != 0)
            {
                await Context.Channel.SendMessageAsync("That group already exists!").ConfigureAwait(false);
                return;
            }
            await PermissionService.CreateGroup(group.Scope, group.GroupName, group.ServerId, group.ChannelId).ConfigureAwait(false);
            await Context.Channel.SendMessageAsync("Created group").ConfigureAwait(false);
        }

        [Command("DeleteGroup"),Summary("Deletes a group")]
        public async Task DeleteGroup(PermissionsGroup group)
        {
            var perm = group.Scope switch
            {
                PermissionsScope.Global => "permissions.deletegroup.global",
                PermissionsScope.Server => "permissions.deletegroup.server",
                PermissionsScope.Channel => "permissions.deletegroup.channel",
                _ => "permissions.deletegroup.any",
            };
            var permsCheck = await PermissionService.HasPermission(perm, Context).ConfigureAwait(false);
            if (!permsCheck.HasValue || !permsCheck.Value)
            {
                await Context.Channel.SendMessageAsync("You do not have permission to do that!").ConfigureAwait(false);
                return;
            }
            if (group.Id == 0)
            {
                await Context.Channel.SendMessageAsync("That group does not exist!").ConfigureAwait(false);
                return;
            }
            await PermissionService.DeleteGroup(group).ConfigureAwait(false);
            await Context.Channel.SendMessageAsync("Deleted group").ConfigureAwait(false);
        }

        [Command("ListGroups"),Summary("List groups for the current context")]
        [RequirePermission("permissions.listgroups", false)]
        public async Task ListGroups(PermissionsScope scope)
        {
            
        }

    }
}
