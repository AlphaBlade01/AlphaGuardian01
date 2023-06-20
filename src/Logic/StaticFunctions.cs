using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace AlphaGuardian01.src.Logic
{
    static public class StaticFunctions
    {
        static public int SecondsFromComponents(int days = 0, int hours = 0, int minutes = 0, int seconds = 0)
        {
            return days * 24 * 3600 + hours * 3600 + minutes * 60 + seconds;
        }

        static public string StringFromTimeComponents(int days = 0, int hours = 0, int minutes = 0, int seconds = 0)
        {
            return $"{(days != 0 ? $"{days}d " : "")} {(hours != 0 ? $"{hours}h " : "")} {(minutes != 0 ? $"{minutes}m " : "")} {(seconds != 0 ? $"{seconds}s " : "")}";
        }

        static public async Task SendEmbed(SocketInteractionContext context, Embed embed)
        {
            await context.Interaction.ModifyOriginalResponseAsync(m =>
            {
                m.Content = null;
                m.Embed = embed;
            });
        }

        static public string FormatModerationString(SocketUser user, string title, bool success, string description, TimeSpan? length = null)
        {
            string timeFromComponents = StringFromTimeComponents(length?.Days ?? 0, length?.Hours ?? 0, length?.Minutes ?? 0, length?.Seconds ?? 0);
            return $"{(success ? ":white_check_mark:" : ":no_entry: ")} **{title} <@{user.Id}> {timeFromComponents} :** `{(success ? description : "Internal Error")}`";
        }
    }
}
