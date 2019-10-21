using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimSarcasm.Services;

namespace TimSarcasm.Util
{
    public class RequirePermissionAttribute : PreconditionAttribute
    {
        public override string ErrorMessage { get; set; }

        public string Permission { get; set; }
        public bool Default { get; set; }

        public RequirePermissionAttribute(string permission, bool def)
        {
            this.Permission = permission;
            this.Default = def;
        }

        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var permService = services.GetRequiredService<PermissionService>();
            var resultNullable = await permService.HasPermission(Permission, context);
            bool result;
            if (!resultNullable.HasValue)
            {
                result = Default;
            }
            else
            {
                result = resultNullable.Value;
            }
            return result ? 
                PreconditionResult.FromSuccess() :
                PreconditionResult.FromError(ErrorMessage ?? "You do not have permission to run this command (" + Permission + ")");
        }
    }
}
