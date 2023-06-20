using Discord.Interactions;
using AlphaGuardian01.src.Logic.Attributes;
using AlphaGuardian01.src.Logic.InteractableComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using Discord;
using MongoDB.Bson.Serialization.Serializers;
using System.Reflection;

namespace AlphaGuardian01.src.Commands.SlashCommands
{
    public class Flags : InteractionModuleBase<SocketInteractionContext>
    {
        private HttpClient _httpClient = new();
        private Dictionary<string, string[]> _countryCodes;
        private Random _random = new();

        const string baseFlagsUrl = "https://flagcdn.com/h240/COUNTRY_CODE.png";

        public enum flagCategories
        {
            world
        }


        private Embed generateEmbed(int round, string flagImageUrl)
        {
            return new EmbedBuilder()
                .WithTitle($"Round {round + 1}")
                .WithImageUrl(flagImageUrl)
                .WithColor(Color.DarkGreen)
                .Build();
        }


        private (Embed, string[]) worldFlags(int round)
        {
            KeyValuePair<string, string[]> element = _countryCodes.ElementAt(_random.Next(_countryCodes.Count));
            string[] countryName = element.Value.Select(alias => alias.ToLower().Trim()).ToArray();
            string flagImageUrl = baseFlagsUrl.Replace("COUNTRY_CODE", element.Key);
            Embed embed = generateEmbed(round, flagImageUrl);
            Console.WriteLine(countryName[0]);

            return (embed, countryName);
        }


        private void initialiseEnvironment()
        {
            //HttpResponseMessage jsonResponse = await _httpClient.GetAsync("https://flagcdn.com/en/codes.json");
            try
            {
                Stream? jsonStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AlphaGuardian01.src.Data.CountryCodes.json");
                string? json;
                using (StreamReader reader = new StreamReader(jsonStream)) { json = reader.ReadToEnd(); }
                _countryCodes = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(json);
            } catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message); 
            } 
            
        }


        public Flags() => initialiseEnvironment();


        [SlashCommand("flags", "test your flags knowledge")]
        [CommandCategory(CommandCategories.Fun)]
        public async Task Execute(flagCategories category = flagCategories.world, int rounds = 10)
        {
            Quiz quiz = category switch
            {
                _ => new(rounds, worldFlags, Context.Channel)
            };

            if (quiz is not null)
                await quiz.BasicQuiz(Context);
        }
    }
}
