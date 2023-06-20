using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace AlphaGuardian01.src.Commands
{
    internal class InteractionHandler
    {
        private readonly InteractionService _interactionService;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        public InteractionHandler(DiscordSocketClient client, InteractionService interactionService, IServiceProvider services) 
        {
            _client = client;
            _interactionService = interactionService;
            _services = services;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
                _client.InteractionCreated += HandleInteraction;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task HandleInteraction(SocketInteraction interaction)
        {
            try
            {
                var context = new SocketInteractionContext(_client, interaction);
                await context.Interaction.RespondAsync("Processing...");
                await _interactionService.ExecuteCommandAsync(context, _services);
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
}
    }
}
