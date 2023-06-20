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
    public class Award : InteractionModuleBase<SocketInteractionContext>
    {
        private DataHandler handler = new DataHandler();

        [SlashCommand("award", "award a user for their actions")]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        [CommandCategory(CommandCategories.Administrator)]
        public async Task Execute(SocketUser user, string reason)
        {
            string moderator_id = Context.User.Id.ToString();
            UserModel data = await handler.GetData(user.Id.ToString());

            var newModel = new ModerationsModel { _id = ObjectId.GenerateNewId(), description = reason, moderator_id = moderator_id, timestamp = DateTime.Now };
            data.awards.Add(newModel);
            bool success = await handler.SetData(user.Id.ToString(), data);

            Embed embed = new EmbedBuilder()
                .WithColor(success ? Color.Green : Color.Red)
                .WithDescription(StaticFunctions.FormatModerationString(user, "Award", success, reason))
                .Build();

            await StaticFunctions.SendEmbed(Context, embed);
        }
    }
}
