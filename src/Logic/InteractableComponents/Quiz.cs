using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlphaGuardian01.src.Logic;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using DnsClient;

namespace AlphaGuardian01.src.Logic.InteractableComponents
{
    public class Quiz : IDisposable
    {
        private readonly int _rounds;
        private int _roundsPassed;
        private Func<int, (Embed, string[])> _questionGenerator;
        private Dictionary<SocketUser, int> _score;
        private ISocketMessageChannel _channel;


        internal async Task OnCorrectAnswer(SocketMessage message)
        {
            SocketUser user = message.Author;
            _score.TryAdd(user, 0);
            _score[user]++;

            _roundsPassed++;
            await message.AddReactionAsync(new Emoji("✅"));

            if (_roundsPassed < _rounds)
                await BasicQuiz(message);
            else
                this.Dispose();
        }


        public Quiz(int rounds, Func<int, (Embed, string[])> questionGenerator, ISocketMessageChannel channel)
        {
            _rounds = rounds;
            _questionGenerator = questionGenerator;
            _roundsPassed = 0;
            _score = new();
            _channel = channel;
        }

        private Embed BasicQuiz()
        {
            (Embed, string[]) questionData = _questionGenerator.Invoke(_roundsPassed);
            ListenerItem listenerItem = new(questionData.Item2, OnCorrectAnswer);
            Listener.AddListener(_channel, listenerItem);

            return questionData.Item1;
        }

        public async Task BasicQuiz(SocketInteractionContext context)
        {
            Embed embed = BasicQuiz();
            await StaticFunctions.SendEmbed(context, embed);
        }

        public async Task BasicQuiz(SocketMessage message)
        {
            Embed embed = BasicQuiz();
            await message.Channel.SendMessageAsync(embed: embed, messageReference: message.Reference);
        }

        public void Dispose()
        {
            Embed scoresEmbed = new EmbedBuilder()
                .WithTitle("Scores")
                .WithDescription(String.Join("\n", _score.Select(kv => $"{kv.Key.Username} : {kv.Value}")))
                .WithColor(Color.DarkGreen)
                .Build();
            _channel.SendMessageAsync(embed: scoresEmbed);
            Listener.RemoveListener(_channel);
        }
    }
}
