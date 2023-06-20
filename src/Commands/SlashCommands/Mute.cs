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
    public class Mute : InteractionModuleBase<SocketInteractionContext>
    {
        private DataHandler handler = new DataHandler();


        [SlashCommand("mute", "Mute a member for a specified time (defaults to 1h)")]
        [DefaultMemberPermissions(GuildPermission.ModerateMembers)]
        [CommandCategory(CommandCategories.Moderator)]
        public async Task Execute(SocketUser user, string reason, int days = 0, int hours = 0, int minutes = 0, int seconds = 0)
        {
            int length_seconds = StaticFunctions.SecondsFromComponents(days, hours, minutes, seconds);
            TimeSpan length = TimeSpan.FromSeconds(length_seconds);
            UserModel data = await handler.GetData(user.Id.ToString());
            IGuildUser guildUser = Context.Guild.GetUser(user.Id);
            string moderator_id = Context.User.Id.ToString();

            var new_model = new ModerationsModel { _id = ObjectId.GenerateNewId(), description = reason, timestamp = DateTime.Now, moderator_id = moderator_id, length = length_seconds > 0 ? length_seconds : 3600 };

            data.mutes.Add(new_model);

            bool success = await handler.SetData(user.Id.ToString(), data);

            Embed embed = new EmbedBuilder()
                .WithColor(success ? Color.Green : Color.Red)
                .WithDescription(StaticFunctions.FormatModerationString(user, "Mute", success, reason, length))
                .Build();

            if (success)
                await guildUser.SetTimeOutAsync(TimeSpan.FromSeconds(length_seconds != 0 ? length_seconds : 3600));

            await StaticFunctions.SendEmbed(Context, embed);
        }
    }
}
