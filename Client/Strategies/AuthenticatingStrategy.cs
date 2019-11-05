using GrpcChat.Contracts;
using System.Threading.Tasks;
using static GrpcChat.Contracts.AuthenticationService;

namespace GrpcChat.Client.Strategies
{
    public class AuthenticatingStrategy
    {
        public async Task<string> RunAsync()
        {
            using var channel = ChannelFactory.CreateAnonymous();
            var client = new AuthenticationService.AuthenticationServiceClient(channel);
            var authToken = "";

            while (string.IsNullOrEmpty(authToken))
            {
                authToken = await TryOnceAsync(client);
            }

            return authToken;
        }

        private async Task<string> TryOnceAsync(AuthenticationServiceClient client)
        {
            var request = new LoginRequest
            {
                Username = ConsoleHelpers.ReadString("Your name"),
                InvitationCode = ConsoleHelpers.ReadString("Invitation code")
            };

            var response = await client.LoginAsync(request);

            if (response.ResultCase == LoginResponse.ResultOneofCase.Success)
            {
                ConsoleHelpers.PrintSuccess("Login successful. JWT acquired.");
                return response.Success.AuthToken;
            }
            else
            {
                ConsoleHelpers.PrintFailure(response.Failure.FailReason);
                return null;
            }
        }
    }
}
