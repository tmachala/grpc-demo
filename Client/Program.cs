using GrpcChat.Client;
using GrpcChat.Client.Strategies;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ConsoleHelpers.PrintWelcome();

            using var tokenSource = new CancellationTokenSource();
            var stoppingToken = tokenSource.Token;

            Console.CancelKeyPress += (sender, e) => tokenSource.Cancel();
            
            //
            // Step 1: Get the authentication token
            //

            var authStrategy = new AuthenticatingStrategy();
            var authToken = await authStrategy.RunAsync();

            //
            // Step 2: Use the authentication token for chatting
            //

            try
            {
                var chatStrategy = new ChattingStrategy();
                await chatStrategy.RunAsync(authToken, stoppingToken);
            }
            catch (Exception) when (stoppingToken.IsCancellationRequested)
            {
                // Just quit
            }
        }
    }
}
