using GrpcChat.Contracts;
using System;
using System.Collections.Generic;

namespace GrpcChat.Client
{
    public static class ConsoleHelpers
    {
        public static void PrintWelcome()
        {
            SetColors(White, Black);
            Console.WriteLine("GRPC CHAT CLIENT");
            SetColors(Gray, Black);
            Console.WriteLine();
            Console.WriteLine("Enter your name and an invitation code to join the chat.");
            Console.WriteLine();
            ResetColors();
        }

        public static void PrintSuccess(string message)
        {
            SetColors(Green, Black);
            Console.WriteLine();
            Console.Write("SUCCESS: ");
            SetColors(White, Black);
            Console.WriteLine(message);
            Console.WriteLine();
            ResetColors();
        }

        public static void PrintFailure(string reason)
        {
            SetColors(Red, Black);
            Console.WriteLine();
            Console.Write("FAIL: ");
            SetColors(White, Black);
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
                SetColors(Gray, Black);
                Console.Write(paddedPrompt);
                SetColors(Yellow, Black);
                Console.Write(" > ");
                SetColors(White, Black);
                answer = Console.ReadLine().Trim();
                Console.CursorTop--;
            }

            SetColors(Gray, Black);
            Console.Write(paddedPrompt + "   ");
            SetColors(White, Black);
            Console.WriteLine(answer);
            ResetColors();

            return answer.Trim();
        }

        public static void PrintMention(Mention mention)
        {
            SetColors(Magenta, Black);
            Console.Write("          " + mention.Sender);
            SetColors(White, Black);
            Console.Write(" has mentioned");

            for (int i = 0; i < mention.MentionedUsers.Count; i++)
            {
                if (i > 0)
                {
                    SetColors(White, Black);
                    Console.Write(" and");
                }

                SetColors(Magenta, Black);
                Console.Write(" " + mention.MentionedUsers[i]);
            }

            Console.WriteLine();
            ResetColors();
        }

        public static void PrintUserEvent(UserEvent userEvent)
        {
            SetColors(Magenta, Black);
            Console.Write("          " + userEvent.Username);
            SetColors(White, Black);
            Console.WriteLine(EventDescriptions[userEvent.EventType]);
            ResetColors();
        }

        public static void PrintMessage(SentMessage message)
        {
            SetColors(Yellow, Black);
            Console.Write((message.Sender + ": ").PadRight(10));
            SetColors(Gray, Black);
            Console.WriteLine(message.Content);
        }

        private static void SetColors(ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
        }

        private static void ResetColors()
        {
            SetColors(Gray, Black);
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
