using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcChat.Contracts;
using GrpcChat.Server.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcChat.Server.Services
{
    public class ChatService : Contracts.ChatService.ChatServiceBase
    {
        public ChatService(ChatRoom chatRoom)
        {
            _chatRoom = chatRoom;
        }

        public override Task<SentMessage> SendMessage(MessageToSend request, ServerCallContext context)
        {
            var sender = GetUserName(context);
            var sentMessage = new SentMessage
            {
                Content = request.Content,
                Sender = sender,
                SentUtc = Timestamp.FromDateTime(DateTime.UtcNow)
            };

            _chatRoom.PushToAllExcept(new Notification { MessageSent = sentMessage }, sender);

            var mentionedUsers = MentionHelpers.Parse(request.Content);

            if (mentionedUsers.Any())
            {
                var mention = new Mention { Sender = sender };
                mention.MentionedUsers.AddRange(mentionedUsers);
                _chatRoom.PushToAllExcept(new Notification { Mention = mention }, sender);
            }

            return Task.FromResult(sentMessage);
        }

        public override async Task Subscribe(SubscribeRequest request, IServerStreamWriter<Notification> responseStream, ServerCallContext context)
        {
            var username = GetUserName(context);
            var queue = _chatRoom.JoinRoom(username);

            try
            {
                foreach (var notification in queue.GetConsumingEnumerable(context.CancellationToken))
                {
                    await responseStream.WriteAsync(notification);
                }
            }
            finally
            {
                _chatRoom.LeaveRoom(username);
            }
        }

        private static string GetUserName(ServerCallContext context)
            => context.GetHttpContext().User.Identity.Name;

        private readonly ChatRoom _chatRoom;
    }
}
