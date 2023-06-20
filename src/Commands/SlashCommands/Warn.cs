using AlphaGuardian01.src.Logic;
using AlphaGuardian01.src.Logic.Attributes;
using AlphaGuardian01.src.Logic.Models;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace AlphaGuardian01.src.Commands.SlashCommands
{
    public class Warn : InteractionModuleBase<SocketInteractionContext>
    {
        static DataHandler handler = new DataHandler();

        [SlashCommand("warn", "warn a user")]
        [DefaultMemberPermissions(GuildPermission.ModerateMembers)]
        [CommandCategory(CommandCategories.Moderator)]
        public async Task Execute(SocketUser user, string reason)
        {
            string user_id = user.Id.ToString();
            string moderator_id = Context.User.Id.ToString();
            DateTime timestamp = DateTime.Now;
            UserModel data = await handler.GetData(user_id);

            var newWarnField = new ModerationsModel { _id = ObjectId.GenerateNewId() };
            newWarnField.description = reason;
            newWarnField.timestamp = timestamp;
            newWarnField.moderator_id = moderator_id;

            data.warnings.Add(newWarnField);
            bool success = await handler.SetData(user_id, data);

            Embed embed = new EmbedBuilder()
                .WithColor(success ? Color.Green : Color.Red)
                .WithDescription(StaticFunctions.FormatModerationString(user, "Warn", success, reason))
                .Build();

            await StaticFunctions.SendEmbed(Context, embed);
        }
    }
}
