using Grpc.Core;
using Grpc.Net.Client;
using GrpcChat.Client;
using GrpcChat.Contracts;
using System;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ConsoleHelpers.PrintWelcome();

            var authToken = await LoginAsync();
            
            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                metadata.Add("Authorization", "Bearer " + authToken);
                return Task.CompletedTask;
            });

            using var authenticatedChannel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
            });


            var client = new ChatService.ChatServiceClient(authenticatedChannel);

            var res = await client.HelloAsync(new HelloRequest { Name = "sdfsd" });

            Console.WriteLine(res.Text);


            //Console.WriteLine("Response: " + reply.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static async Task<string> LoginAsync()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new AuthenticationService.AuthenticationServiceClient(channel);

            var request = new LoginRequest
            {
                Username = ConsoleHelpers.ReadString("Your name"),
                InvitationCode = ConsoleHelpers.ReadString("Invitation code")
            };

            var response = await client.LoginAsync(request);

            if (response.ResultCase == LoginResponse.ResultOneofCase.Success)
            {
                ConsoleHelpers.PrintSuccess("Login successful. JWT token acquired.");
                return response.Success.AuthToken;
            }
            else
            {
                Console.WriteLine("FAIL: " + response.Failure.FailReason);
                return null;
            }
        }

        //private static async Task TaskJoinRoomAsync(ChatServiceClient client)
        //{
        //    while (true)
        //    {
        //        if (await TryJoinOnceAsync(client))
        //            return;
        //    }
        //}

        //private static async Task<bool> TryJoinOnceAsync(ChatServiceClient client)
        //{
        //    ConsoleHelpers.PrintWelcome();

        //    var joinRequest = new JoinRoomRequest
        //    {
        //        Username = ConsoleHelpers.ReadString("Your name"),
        //        InvitationCode = ConsoleHelpers.ReadString("Invitation code")
        //    };

        //    var joinResponse = await client.JoinRoomAsync(joinRequest);

        //    if (joinResponse.ResultCase == JoinRoomResponse.ResultOneofCase.Success)
        //    {
        //        return true;
        //    }

        //    return false;
        //}
    }
}
