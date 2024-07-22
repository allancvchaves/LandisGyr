using EndpointManager.Interfaces;
using System;

namespace EndpointManager.Services
{
    public class UserInputService : IUserInputService
    {
        public string GetInput(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }

        public bool TryGetIntInput(string prompt, out int value)
        {
            Console.WriteLine(prompt);
            string input = Console.ReadLine();
            return int.TryParse(input, out value);
        }

        public bool TryGetEnumInput<T>(string prompt, out T result) where T : Enum
        {
            Console.WriteLine(prompt);
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                Console.WriteLine($"{(int)item} for {item}");
            }

            string input = Console.ReadLine();
            if (int.TryParse(input, out int value) && Enum.IsDefined(typeof(T), value))
            {
                result = (T)Enum.ToObject(typeof(T), value);
                return true;
            }

            result = default;
            return false;
        }

        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public bool ConfirmAction(string prompt)
        {
            Console.WriteLine(prompt);
            string input = Console.ReadLine();
            return string.Equals(input, "y", StringComparison.OrdinalIgnoreCase);
        }

    }
}
