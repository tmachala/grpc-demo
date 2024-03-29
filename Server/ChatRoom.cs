﻿using GrpcChat.Contracts;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace GrpcChat.Server
{
    public class ChatRoom
    {
        public BlockingCollection<Notification> JoinRoom(string username)
        {
            var queue = new BlockingCollection<Notification>();
            var success = _notificationQueues.TryAdd(username, queue);

            if (!success)
            {
                throw new ApplicationException($"Another user named '{username}' in already in the room'!");
            }

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

        private readonly ConcurrentDictionary<string, BlockingCollection<Notification>> _notificationQueues = new ConcurrentDictionary<string, BlockingCollection<Notification>>();
    }
}
