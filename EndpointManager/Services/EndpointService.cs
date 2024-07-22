using EndpointManager.Enums;
using EndpointManager.Exceptions;
using EndpointManager.Interfaces;
using EndpointManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EndpointManager.Services
{
    public class EndpointService : IEndpointService
    {
        private readonly List<Endpoint> endpoints;
        private readonly IUserInputService userInputService;

        public EndpointService(List<Endpoint> endpoints, IUserInputService userInputService)
        {
            this.endpoints = endpoints;
            this.userInputService = userInputService;
        }

        public void InsertEndpoint(Endpoint endpoint)
        {
            endpoints.Add(endpoint);
            userInputService.DisplayMessage("Endpoint created successfully.");
        }

        public void GetInfoNewEndpoint()
        {
            var serialNumber = userInputService.GetInput("Please input the endpoint serial number:");
            if (FindBySerialNumber(serialNumber) != null)
            {
                throw new DuplicateSerialNumberException("There is already an endpoint with that serial number, please try again.");
            }

            if (!userInputService.TryGetEnumInput("Please input meter model ID, the options are:", out Models modelId))
            {
                userInputService.DisplayMessage("Invalid value for meter model ID, please try again.");
                return;
            }

            if (!userInputService.TryGetIntInput("Please input meter number:", out int meterNumberValue))
            {
                throw new InvalidValueException("Invalid value for meter number, please try again.");
            }

            var meterVersion = userInputService.GetInput("Please input meter firmware version:");

            if (!userInputService.TryGetEnumInput("Please input switch state, the options are:", out States switchState))
            {
                userInputService.DisplayMessage("Invalid value for switch state, please try again.");
                return;
            }
            InsertEndpoint(new Endpoint
            {
                SerialNumber = serialNumber,
                MeterModelId = modelId,
                MeterNumber = meterNumberValue,
                MeterFirmwareVersion = meterVersion,
                SwitchState = switchState
            });
        }

        public void EditEndpoint()
        {
            try
            {
                var serialNumber = userInputService.GetInput("Please input the endpoint serial number:");
                var endpoint = FindBySerialNumber(serialNumber);
                if (endpoint == null)
                {
                    throw new EndpointNotFoundException("There is no endpoint with that serial number, please try again.");
                }

                if (!userInputService.TryGetEnumInput("Please input switch state, the options are:", out States switchState))
                {
                    userInputService.DisplayMessage("Invalid value for switch state, please try again.");
                    return;
                }
                endpoint.SwitchState = switchState;
                userInputService.DisplayMessage("Endpoint updated successfully.");
            }
            catch (Exception ex)
            {
                userInputService.DisplayMessage($"An error occurred while editing the endpoint: {ex.Message}");
            }

        }

        public void DeleteEndpoint()
        {
            try
            {
                var serialNumber = userInputService.GetInput("Please input the endpoint serial number:");
                var endpoint = FindBySerialNumber(serialNumber);
                if (endpoint == null)
                {
                    throw new EndpointNotFoundException("There is no endpoint with that serial number, please try again.");
                }

                var inputOption = userInputService.GetInput("Do you want to delete this endpoint? (y/n)");
                if (inputOption != null && string.Equals(inputOption, "y", StringComparison.OrdinalIgnoreCase))
                {
                    endpoints.Remove(endpoint);
                    userInputService.DisplayMessage("Endpoint deleted.");
                }
            }
            catch (Exception ex)
            {
                userInputService.DisplayMessage($"An error occurred while deleting the endpoint: {ex.Message}");
            }
        }

        public void ListAllEndpoints()
        {
            try
            {
                if (!endpoints.Any())
                {
                    userInputService.DisplayMessage("There are no endpoints so far.");
                    return;
                }

                userInputService.DisplayMessage("Here are the endpoints:");
                endpoints.ForEach(endpoint =>
                {
                    PrintProperties(endpoint);
                    userInputService.DisplayMessage("--------------------------");
                });
            }
            catch (Exception ex)
            {
                userInputService.DisplayMessage($"An error occurred while listing the endpoints: {ex.Message}");
            }
        }

        public void FindEndpoint()
        {
            try
            {
                var serialNumber = userInputService.GetInput("Please input the endpoint serial number:");
                var endpoint = FindBySerialNumber(serialNumber);
                if (endpoint == null)
                {
                    throw new EndpointNotFoundException("There is no endpoint with that serial number, please try again.");
                }
                PrintProperties(endpoint);
            }
            catch (Exception ex)
            {
                userInputService.DisplayMessage($"An error occurred while finding the endpoint: {ex.Message}");
            }
        }

        private Endpoint FindBySerialNumber(string serialNumber)
        {
            try
            {
                return endpoints.SingleOrDefault(x => x.SerialNumber == serialNumber);
            }
            catch (Exception ex)
            {
                userInputService.DisplayMessage($"An error occurred while finding by serial number: {ex.Message}");
                return null;
            }
        }

        private void PrintProperties<T>(T obj)
        {
            try
            {
                foreach (var property in typeof(T).GetProperties())
                {
                    var value = property.GetValue(obj);
                    userInputService.DisplayMessage($"{property.Name}: {value}");
                }
            }
            catch (Exception ex)
            {
                userInputService.DisplayMessage($"An error occurred while printing properties: {ex.Message}");
            }
        }
    }
}
