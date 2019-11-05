using Grpc.Core;
using Grpc.Net.Client;
using System.Threading.Tasks;

namespace GrpcChat.Client
{
    public static class ChannelFactory
    {
        public static GrpcChannel CreateAnonymous()
        {
            return GrpcChannel.ForAddress(ServerAddress);
        }

        public static GrpcChannel CreateWithAuthToken(string authToken)
        {
            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                metadata.Add("Authorization", "Bearer " + authToken);
                return Task.CompletedTask;
            });

            return GrpcChannel.ForAddress(ServerAddress, new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
            });
        }

        private const string ServerAddress = "https://localhost:5001";
    }
}
