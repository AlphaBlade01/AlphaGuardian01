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
    public class Ban : InteractionModuleBase<SocketInteractionContext>
    {
        DataHandler handler = new DataHandler();


        [SlashCommand("ban", "bans specified user for specified time if provided")]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        [CommandCategory(CommandCategories.Administrator)]
        public async Task Execute(SocketUser user, string reason, int days = 0, int hours = 0, int minutes = 0, int seconds = 0)
        {
            UserModel data = await handler.GetData(user.Id.ToString());
            int seconds_length = StaticFunctions.SecondsFromComponents(days, hours, minutes, seconds);
            TimeSpan length = TimeSpan.FromSeconds(seconds_length);

            var newBan = new ModerationsModel { _id = ObjectId.GenerateNewId(), description = reason, moderator_id = Context.User.Id.ToString(), timestamp = DateTime.Now };
            if (seconds_length > 0)
                newBan.length = seconds_length;

            data.bans.Add(newBan);
            bool success = await handler.SetData(user.Id.ToString(), data);

            Embed embed = new EmbedBuilder()
                .WithColor(success ? Color.Green : Color.Red)
                .WithDescription(StaticFunctions.FormatModerationString(user, "Ban", success, reason, length))
                .Build();

            await StaticFunctions.SendEmbed(Context, embed);

            if (success)
            {
                await Context.Guild.AddBanAsync(user, 1, reason);

                if (seconds_length == 0)
                    return;

                await Task.Delay(seconds_length * 1000);
                await Context.Guild.RemoveBanAsync(user);
            }
        }
    }
}