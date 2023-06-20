using AlphaGuardian01.src.Logic;
using AlphaGuardian01.src.Logic.Attributes;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace AlphaGuardian01.src.Commands.SlashCommands
{
    public class Kick : InteractionModuleBase<SocketInteractionContext>
    {

        [SlashCommand("kick", "kick a user from the server")]
        [DefaultMemberPermissions(GuildPermission.ModerateMembers)]
        [CommandCategory(CommandCategories.Moderator)]
        public async Task Execute(SocketUser user, string reason)
        {
            IGuildUser guildUser = Context.Guild.GetUser(user.Id);
            bool success = true;

            try
            {
                await guildUser.KickAsync(reason);
            } catch (Exception ex)
            {
                success = false;
                Console.WriteLine(ex.ToString());
            }

            Embed embed = new EmbedBuilder()
                .WithColor(success ? Color.Green : Color.Red)
                .WithDescription(StaticFunctions.FormatModerationString(user, "Kick", success, reason))
                .Build();

            await StaticFunctions.SendEmbed(Context, embed);
        }
    }
}
