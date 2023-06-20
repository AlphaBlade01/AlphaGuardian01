using AlphaGuardian01;
using AlphaGuardian01.src.Logic;
using AlphaGuardian01.src.Commands;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

DiscordSocketClient client;
InteractionService interactionService;
IHost host;

static Task Log(LogMessage message)
{
    Console.WriteLine(message);
    return Task.CompletedTask;
}

Task Ready()
{
    interactionService.RegisterCommandsGloballyAsync();
    return Task.CompletedTask;
}

try
{
    Stream? dotenv = Assembly.GetExecutingAssembly().GetManifestResourceStream("AlphaGuardian01..env");
    using (StreamReader reader = new StreamReader(dotenv))
        DotEnv.Load(reader.ReadToEnd());
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}


DiscordSocketConfig clientConfig = new DiscordSocketConfig
{
    GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
};


host = Host.CreateDefaultBuilder()
    .ConfigureServices(services => services
        .AddSingleton(clientConfig)
        .AddSingleton<DiscordSocketClient>()
        .AddSingleton<InteractionService>()
        .AddSingleton<InteractionHandler>())
    .Build();

interactionService = host.Services.GetRequiredService<InteractionService>();
client = host.Services.GetRequiredService<DiscordSocketClient>();

await host.Services.GetRequiredService<InteractionHandler>().InitialiseAsync();

client.Log += Log;
client.Ready += Ready;

await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("TOKEN"));
await client.StartAsync();
Listener.StartListen(client);

await Task.Delay(-1);