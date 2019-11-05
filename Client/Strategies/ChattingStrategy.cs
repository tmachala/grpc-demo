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
            var channel = ChannelFactory.CreateWithAuthToken(authToken);
            var client = new ChatServiceClient(channel);

            var subscription = client.Subscribe(new SubscribeRequest(), cancellationToken: stoppingToken);

            // TODO
            //var res2 = await client.SendMessageAsync(new MessageToSend { Content = "dsfs @john @alice" });
            //await client.SendMessageAsync(new MessageToSend { Content = "2" });
            //await client.SendMessageAsync(new MessageToSend { Content = "3e" });
            //await client.SendMessageAsync(new MessageToSend { Content = "4" });

            // Listen for incomming notification in a separate thread (otherwise, we couldn't type new messages)
            var listeningTask = Task.Run(async () => await ListenForNotificationsAsync(client, stoppingToken));

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var content = Console.ReadLine();
                    await client.SendMessageAsync(new MessageToSend { Content = content }, cancellationToken: stoppingToken);
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
                    // TODO
                    Console.WriteLine(notification.UserEvent.Username + ": " + notification.UserEvent.EventType.ToString());
                }
            }
        }
    }
}
