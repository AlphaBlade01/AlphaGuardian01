using AlphaGuardian01.src.Logic;
using AlphaGuardian01.src.Logic.Attributes;
using Discord;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AlphaGuardian01.src.Commands.SlashCommands
{
    public class Help : InteractionModuleBase<SocketInteractionContext>
    {
        private Embed EmbedFromCategorisedCommands(Dictionary<string, string> commands)
        {
            EmbedBuilder embed = new EmbedBuilder()
                .WithColor(Color.Magenta)
                .WithTitle("Help");

            foreach (var command in commands)
            {
                embed.AddField(command.Key, command.Value);
            }
            return embed.Build();
        }

        [SlashCommand("help", "get a list of commands")]
        [CommandCategory(CommandCategories.Miscellaneous)]
        public async Task Execute()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var attributeType = typeof(CommandCategory);
            var attributeImplementedObjects = assembly
                .GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttribute(attributeType) != null)
                .ToList();
            var categoryCommandPairs = new Dictionary<string, string>();

            foreach (var method in attributeImplementedObjects)
            {
                var category = method.GetCustomAttribute<CommandCategory>().Category;
                var slashCommandAttribute = method.GetCustomAttribute<SlashCommandAttribute>();

                if (categoryCommandPairs.TryGetValue(category.ToString(), out string cmds))
                    categoryCommandPairs[category.ToString()] = cmds + $", `{slashCommandAttribute.Name}`";
                else
                    categoryCommandPairs.Add(category.ToString(), $"`{slashCommandAttribute.Name}`");
            }

            Embed embed = EmbedFromCategorisedCommands(categoryCommandPairs);
            await StaticFunctions.SendEmbed(Context, embed);
        }
    }
}
