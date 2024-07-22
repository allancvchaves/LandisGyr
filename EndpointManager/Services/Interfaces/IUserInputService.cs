using System;

namespace EndpointManager.Interfaces
{
    public interface IUserInputService
    {
        string GetInput(string prompt);
        bool TryGetIntInput(string prompt, out int value);
        bool TryGetEnumInput<T>(string prompt, out T result) where T : Enum;
        void DisplayMessage(string message);
        bool ConfirmAction(string prompt);
    }
}
