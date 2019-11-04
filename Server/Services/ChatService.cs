using Grpc.Core;
using GrpcChat.Contracts;
using System.Threading.Tasks;
using static GrpcChat.Contracts.ChatService;

namespace GrpcChat.Server.Services
{
    public class ChatService : ChatServiceBase
    {
        public override Task<HelloResponse> Hello(HelloRequest request, ServerCallContext context)
        {
            var username = context.GetHttpContext().User.Identity.Name;
            return Task.FromResult(new HelloResponse { Text = "Hi " + username });
        }
    }
}
