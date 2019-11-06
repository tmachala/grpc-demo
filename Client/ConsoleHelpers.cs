using GrpcChat.Contracts;
using System;
using System.Collections.Generic;

namespace GrpcChat.Client
{
    public static class ConsoleHelpers
    {
        public static void PrintWelcome()
        {
            SetColor(White);
            Console.WriteLine("GRPC CHAT CLIENT");
            SetColor(Gray);
            Console.WriteLine();
            Console.WriteLine("Enter your name and an invitation code to join the chat.");
            Console.WriteLine("The invitation code is \"1234\".");
            Console.WriteLine();
            Console.WriteLine("Pro Tip: You can mention others via @.");
            Console.WriteLine();
            ResetColors();
        }

        public static void PrintSuccess(string message)
        {
            SetColor(Green);
            Console.WriteLine();
            Console.Write("SUCCESS: ");
            SetColor(White);
            Console.WriteLine(message);
            Console.WriteLine();
            ResetColors();
        }

        public static void PrintFailure(string reason)
        {
            SetColor(Red);
            Console.WriteLine();
            Console.Write("FAIL: ");
            SetColor(White);
            Console.WriteLine(reason);
            Console.WriteLine();
            ResetColors();
        }

        public static string ReadString(string prompt)
        {
            var answer = "";

            var paddedPrompt = prompt.Length < 15
                ? prompt.PadLeft(15)
                : prompt;

            while (string.IsNullOrEmpty(answer))
            {
                SetColor(Gray);
                Console.Write(paddedPrompt);
                SetColor(Yellow);
                Console.Write(" > ");
                SetColor(White);
                answer = Console.ReadLine().Trim();
                Console.CursorTop--;
            }

            SetColor(Gray);
            Console.Write(paddedPrompt + "   ");
            SetColor(White);
            Console.WriteLine(answer);
            ResetColors();

            return answer.Trim();
        }

        public static void PrintMention(Mention mention)
        {
            SetColor(Magenta);
            Console.Write(mention.Sender);
            SetColor(White);
            Console.Write(" has mentioned");

            for (int i = 0; i < mention.MentionedUsers.Count; i++)
            {
                if (i > 0)
                {
                    SetColor(White);
                    Console.Write(" and");
                }

                SetColor(Magenta);
                Console.Write(" " + mention.MentionedUsers[i]);
            }

            Console.WriteLine();
            ResetColors();
        }

        public static void PrintUserEvent(UserEvent userEvent)
        {
            SetColor(Magenta);
            Console.Write(userEvent.Username);
            SetColor(White);
            Console.WriteLine(EventDescriptions[userEvent.EventType]);
            ResetColors();
        }

        public static void PrintMessage(SentMessage message, bool isOwnMessage = false)
        {
            if (isOwnMessage)
                Console.CursorTop--;

            SetColor(Yellow);
            Console.Write(message.Sender + ": ");
            SetColor(White);
            Console.WriteLine(message.Content);
        }

        private static void SetColor(ConsoleColor foreground)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = Black;
        }

        private static void ResetColors()
        {
            SetColor(Gray);
        }

        private static readonly IReadOnlyDictionary<UserEventType, string> EventDescriptions = new Dictionary<UserEventType, string>
        {
            [UserEventType.JoinedRoom] = " joined the room",
            [UserEventType.LeftRoom] = " left the room"
        };

        private const ConsoleColor White = ConsoleColor.White;
        private const ConsoleColor Black = ConsoleColor.Black;
        private const ConsoleColor Green = ConsoleColor.Green;
        private const ConsoleColor Yellow = ConsoleColor.Yellow;
        private const ConsoleColor Gray = ConsoleColor.Gray;
        private const ConsoleColor Magenta = ConsoleColor.Magenta;
        private const ConsoleColor Red = ConsoleColor.Red;
    }
}
