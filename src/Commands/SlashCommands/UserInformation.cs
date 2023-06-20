using AlphaGuardian01.src.Logic;
using AlphaGuardian01.src.Logic.Attributes;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaGuardian01.src.Commands.SlashCommands
{
    public class UserInformation : InteractionModuleBase<SocketInteractionContext>
    {
        

        [SlashCommand("user-information", "Get user information")]
        [CommandCategory(CommandCategories.Miscellaneous)]
        public async Task Execute(SocketUser member = null)
        {
            SocketUser user = member ?? Context.User;
            var embed = new EmbedBuilder()
                .WithTitle($"{user.Username}#{user.Discriminator}")
                .WithThumbnailUrl(user.GetAvatarUrl())
                .AddField("Roles", string.Join(", ", (user as SocketGuildUser).Roles.Select( r => r.Mention.ToString() )))
                .WithColor(Color.Blue)
                .AddField("Created", String.Format("<t:{0}:f>", user.CreatedAt.ToUnixTimeSeconds()))
                .AddField("User ID", user.Id.ToString())
                .Build();

            await StaticFunctions.SendEmbed(Context, embed);
        }
    }
}
