using EndpointManager.Enums;
using EndpointManager.Exceptions;
using EndpointManager.Interfaces;
using EndpointManager.Model;
using System;

namespace EndpointManager.Services
{
    public class UserInputService : IUserInputService
    {
        private IEndpointService _endpointService;

        public UserInputService()
        {
        }

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

        public virtual void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public bool ConfirmAction(string prompt)
        {
            Console.WriteLine(prompt);
            string input = Console.ReadLine();
            return string.Equals(input, "y", StringComparison.OrdinalIgnoreCase);
        }

        public void PromptNewEndpoint()
        {
            var serialNumber = GetInput("Please input the endpoint serial number:");
            if (_endpointService.FindBySerialNumber(serialNumber) != null)
            {
                throw new DuplicateSerialNumberException("There is already an endpoint with that serial number, please try again.");
            }

            if (!TryGetEnumInput("Please input meter model ID, the options are:", out Models modelId))
            {
                DisplayMessage("Invalid value for meter model ID, please try again.");
                return;
            }

            if (!TryGetIntInput("Please input meter number:", out int meterNumberValue))
            {
                throw new InvalidValueException("Invalid value for meter number, please try again.");
            }

            var meterVersion = GetInput("Please input meter firmware version:");

            if (!TryGetEnumInput("Please input switch state, the options are:", out States switchState))
            {
                DisplayMessage("Invalid value for switch state, please try again.");
                return;
            }
            _endpointService.InsertEndpoint(new Endpoint
            {
                SerialNumber = serialNumber,
                MeterModelId = modelId,
                MeterNumber = meterNumberValue,
                MeterFirmwareVersion = meterVersion,
                SwitchState = switchState
            });
        }

        public void PromptEditEndpoint()
        {
            var serialNumber = GetInput("Please input the endpoint serial number:");
            var endpoint = _endpointService.FindBySerialNumber(serialNumber);
            if (endpoint == null)
            {
                throw new EndpointNotFoundException("There is no endpoint with that serial number, please try again.");
            }

            if (!TryGetEnumInput("Please input switch state, the options are:", out States switchState))
            {
                DisplayMessage("Invalid value for switch state, please try again.");
                return;
            }
            _endpointService.EditEndpoint(endpoint, switchState);
        }

        public void PromptDeleteEndpoint()
        {
            try
            {
                var serialNumber = GetInput("Please input the endpoint serial number:");
                var endpoint = _endpointService.FindBySerialNumber(serialNumber);
                if (endpoint == null)
                {
                    throw new EndpointNotFoundException("There is no endpoint with that serial number, please try again.");
                }

                var inputOption = GetInput("Do you want to delete this endpoint? (y/n)");
                if (inputOption != null && string.Equals(inputOption, "y", StringComparison.OrdinalIgnoreCase))
                {
                    _endpointService.DeleteEndpoint(endpoint);
                }
            }
            catch (Exception ex)
            {
                DisplayMessage($"An error occurred while deleting the endpoint: {ex.Message}");
            }
        }
        public void SetEndpointService(EndpointService endpointService)
        {
            _endpointService = endpointService;
        }
    }
}
