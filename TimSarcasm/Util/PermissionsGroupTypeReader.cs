using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimSarcasm.Models;
using TimSarcasm.Services;

namespace TimSarcasm.Util
{
    public class PermissionsGroupTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            var permissionService = services.GetRequiredService<PermissionService>();
            var name = input.Substring(1);
            PermissionsGroup group;
            switch(input.Substring(0, 1))
            {
                case "%": // global group
                    group = permissionService.GetGroup(PermissionsScope.Global, name);
                    if (group == null)
                    {
                        group = new PermissionsGroup()
                        {
                            GroupName = name,
                            Scope = PermissionsScope.Global
                        };
                    }
                    return Task.FromResult(TypeReaderResult.FromSuccess(group));
                case "!": // server group
                    group = permissionService.GetGroup(PermissionsScope.Server, name, context.Guild.Id);
                    if (group == null)
                    {
                        group = new PermissionsGroup()
                        {
                            GroupName = name,
                            Scope = PermissionsScope.Server,
                            ServerId = context.Guild.Id
                        };
                    }
                    return Task.FromResult(TypeReaderResult.FromSuccess(group));
                case "^": // channel group
                    group = permissionService.GetGroup(PermissionsScope.Channel, name, context.Guild.Id, context.Channel.Id);
                    if (group == null)
                    {
                        group = new PermissionsGroup()
                        {
                            GroupName = name,
                            Scope = PermissionsScope.Channel,
                            ServerId = context.Guild.Id,
                            ChannelId = context.Channel.Id
                        };
                    }
                    return Task.FromResult(TypeReaderResult.FromSuccess(group));
                default:
                    return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Could not figure out the group type."));
            }
        }
    }
}
