using AlphaGuardian01.src.Logic;
using AlphaGuardian01.src.Logic.Attributes;
using AlphaGuardian01.src.Logic.Models;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlphaGuardian01.src.Commands.SlashCommands
{
    public class View : InteractionModuleBase<SocketInteractionContext>
    {
        static DataHandler handler = new DataHandler();
        public enum ViewItems
        {
            awards,
            bans,
            mutes,
            warnings
        }

        private static Embed GetEmbedFromData(SocketUser user, UserModel data, string title)
        {
            EmbedBuilder embed = new EmbedBuilder()
                .WithAuthor(user.ToString(), user.GetAvatarUrl())
                .WithTitle(title)
                .WithColor(Color.DarkMagenta);

            List<ModerationsModel> moderations = (List<ModerationsModel>) data.GetType().GetProperty(title)?.GetValue(data) ?? new List<ModerationsModel>();
            moderations.ForEach(mod =>
            {
                TimeSpan length = TimeSpan.FromSeconds(mod.length ?? 0);
                string formattedLength = StaticFunctions.StringFromTimeComponents(length.Days, length.Hours, length.Minutes, length.Seconds);
                embed.AddField($"`{mod.description}`", $"**Moderator:** <@{mod.moderator_id}>\n**Timestamp:** {mod.timestamp}{(mod.length != null ? $"\n**Length:** {formattedLength}" : "")}");
            });

            if (embed.Fields.Count == 0)
                embed.WithDescription("No information");

            return embed.Build();
        }


        [SlashCommand("view", "View moderations on a user")]
        [DefaultMemberPermissions(GuildPermission.ModerateMembers)]
        [CommandCategory(CommandCategories.Moderator)]
        public async Task Execute(SocketUser user, ViewItems item)
        {
            UserModel data = await handler.GetData(user.Id.ToString());
            Embed embed = GetEmbedFromData(user, data, item.ToString());
            await StaticFunctions.SendEmbed(Context, embed);
        }
    }
}
