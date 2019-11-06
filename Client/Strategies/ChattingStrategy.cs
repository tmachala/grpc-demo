using Grpc.Core;
using GrpcChat.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;
using static GrpcChat.Contracts.ChatService;

namespace GrpcChat.Client.Strategies
{
    public class ChattingStrategy
    {
        public async Task RunAsync(string authToken, CancellationToken stoppingToken)
        {
            using var channel = ChannelFactory.CreateWithAuthToken(authToken);
            var client = new ChatServiceClient(channel);

            // Listen for incomming notification in a separate thread.
            // Otherwise, the console would be blocked and we couldn't type new messages.
            var listeningTask = Task.Run(async () => await ListenForNotificationsAsync(client, stoppingToken));

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var content = Console.ReadLine();
                    var sentMessage = await client.SendMessageAsync(new MessageToSend { Content = content }, cancellationToken: stoppingToken);
                    ConsoleHelpers.PrintMessage(sentMessage, isOwnMessage: true);
                }
            }
            catch (Exception) when (stoppingToken.IsCancellationRequested)
            {
                // Just quit
            }
        }

        private async Task ListenForNotificationsAsync(ChatServiceClient client, CancellationToken stoppingToken)
        {
            var subscription = client.Subscribe(new SubscribeRequest(), cancellationToken: stoppingToken);

            await foreach (var notification in subscription.ResponseStream.ReadAllAsync(stoppingToken))
            {
                if (notification.EventCase == Notification.EventOneofCase.MessageSent)
                {
                    ConsoleHelpers.PrintMessage(notification.MessageSent);
                }
                else if (notification.EventCase == Notification.EventOneofCase.Mention)
                {
                    ConsoleHelpers.PrintMention(notification.Mention);
                }
                else if (notification.EventCase == Notification.EventOneofCase.UserEvent)
                {
                    ConsoleHelpers.PrintUserEvent(notification.UserEvent);
                }
            }
        }
    }
}
