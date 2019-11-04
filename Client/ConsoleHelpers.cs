using System;

namespace GrpcChat.Client
{
    public static class ConsoleHelpers
    {
        public static void PrintWelcome()
        {
            SetColors(White, Black);
            Console.WriteLine("----------  GRPC CHAT CLIENT  ----------");
            SetColors(Gray, Black);
            Console.WriteLine();
            Console.WriteLine("Please enter your name and an invitation code to join the chat room.");
            Console.WriteLine();
        }

        public static void PrintSuccess(string message)
        {
            SetColors(Green, Black);
            Console.WriteLine();
            Console.Write("SUCCESS: ");
            SetColors(White, Black);
            Console.WriteLine(message);
            Console.WriteLine();
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

            return answer.Trim();
        }

        private static void SetColors(ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
        }

        private const ConsoleColor White = ConsoleColor.White;
        private const ConsoleColor Black = ConsoleColor.Black;
        private const ConsoleColor Green = ConsoleColor.Green;
        private const ConsoleColor Yellow = ConsoleColor.Yellow;
        private const ConsoleColor Gray = ConsoleColor.Gray;
    }
}
