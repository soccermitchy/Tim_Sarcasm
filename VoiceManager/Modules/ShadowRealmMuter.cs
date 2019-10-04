using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceManager.Modules
{
    [Group("Utility")]
    public class ShadowRealmMuter : ModuleBase<SocketCommandContext>
    {
        [Command("hidetheshadowrealm")]
        [Summary("Hides the shadow realm from your user, for when it gets removed and recreated.")]
        [Alias("test")]
        public async Task HideTheShadowRealm()
        {
            var user = Context.User as IGuildUser;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "No Shadow Realm");
            if (user.RoleIds.Contains(role.Id))
                await user.RemoveRoleAsync(role);
            else
                await user.AddRoleAsync(role);
            await ReplyAsync("Done");
        }
    }
}
