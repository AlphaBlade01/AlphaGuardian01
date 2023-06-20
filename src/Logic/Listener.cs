using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlphaGuardian01.src.Logic
{
    public class ListenerItem
    {
        public string[] Value { get; }
        public Func<SocketMessage, Task> Handler { get; }

        public ListenerItem(string[] value, Func<SocketMessage, Task> handler)
        {
            Value = value;
            Handler = handler;
        }
    }

    public static class Listener
    {
        public static Dictionary<IMessageChannel, List<ListenerItem>> PendingMessages = new Dictionary<IMessageChannel, List<ListenerItem>>();

        public static void AddListener(IMessageChannel channel, ListenerItem item)
        {
            if (!PendingMessages.ContainsKey(channel))
                PendingMessages.Add(channel, new List<ListenerItem>());

            PendingMessages[channel].Add(item);
        }


        public static void RemoveListener(IMessageChannel channel, ListenerItem? item = null)
        {
            if (!PendingMessages.ContainsKey(channel)) return;

            if (item == null)
                PendingMessages.Remove(channel);
            else
                PendingMessages[channel].Remove(item);
        }


        public static void StartListen(DiscordSocketClient client)
        {
            client.MessageReceived += async (SocketMessage message) =>
            {
                try
                {
                    string guess = message.Content.ToLower().Trim();

                    if (!PendingMessages.ContainsKey(message.Channel) | message.Author.IsBot) return;
                    PendingMessages.TryGetValue(message.Channel, out List<ListenerItem>? pendingValues);

                    ListenerItem? pendingValue = pendingValues?.Find(item => item.Value.Contains(guess));
                    if (pendingValue is null) return;

                    PendingMessages.Remove(message.Channel);
                    await pendingValue.Handler.Invoke(message);
                } catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            };
        }
    }
}
