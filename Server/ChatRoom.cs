using GrpcChat.Contracts;
using System.Collections.Concurrent;
using System.Linq;

namespace GrpcChat.Server
{
    public class ChatRoom
    {
        public BlockingCollection<Notification> JoinRoom(string username)
        {
            var queue = new BlockingCollection<Notification>();
            _notificationQueues.TryAdd(username, queue);

            var notification = CreateUserEventNotification(username, UserEventType.JoinedRoom);
            PushToAllExcept(notification, username);

            return queue;
        }

        public void LeaveRoom(string username)
        {
            _notificationQueues.TryRemove(username, out _);

            var notification = CreateUserEventNotification(username, UserEventType.LeftRoom);
            PushToAllExcept(notification, username);
        }

        public void PushToAllExcept(Notification notification, string exceptUsername)
        {
            var queuesToAddTo = _notificationQueues.Where(q => q.Key != exceptUsername);

            foreach (var q in queuesToAddTo)
            {
                q.Value.Add(notification);
            }
        }

        private static Notification CreateUserEventNotification(string username, UserEventType eventType)
        {
            return new Notification
            {
                UserEvent = new UserEvent
                {
                    Username = username,
                    EventType = eventType
                }
            };
        }

        //
        // TODO: Change to queue?
        // Why are there multiple instances? It's registered as singleton!
        //
        private readonly ConcurrentDictionary<string, BlockingCollection<Notification>> _notificationQueues = new ConcurrentDictionary<string, BlockingCollection<Notification>>();
    }
}
