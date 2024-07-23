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

        public void EditEndpoint(Endpoint endpoint, States newState)
        {
            try
            {
                endpoint.SwitchState = newState;
                userInputService.DisplayMessage("Endpoint updated successfully.");
            }
            catch (Exception ex)
            {
                userInputService.DisplayMessage($"An error occurred while editing the endpoint: {ex.Message}");
            }

        }

        public void DeleteEndpoint(Endpoint endpoint)
        {
            endpoints.Remove(endpoint);
            userInputService.DisplayMessage("Endpoint deleted.");
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

        public Endpoint FindBySerialNumber(string serialNumber)
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
