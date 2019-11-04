using Grpc.Core;
using GrpcChat.Contracts;
using System.Threading.Tasks;
using static GrpcChat.Contracts.AuthenticationService;

namespace GrpcChat.Server.Services
{
    public class AuthenticationService : AuthenticationServiceBase
    {
        public override Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            var response = new LoginResponse();

            if (request.InvitationCode == ExpectedInvitationCode)
            {
                var jwt = SimpleAuthentication.CreateJwtToken(request.Username);
                response.Success = new LoginSuccess { AuthToken = jwt };
            }
            else
            {
                response.Failure = new LoginFailure { FailReason = "Invalid invitation code!" };
            }

            return Task.FromResult(response);
        }

        private const string ExpectedInvitationCode = "1234";
    }
}
