using EndpointManager.Exceptions;
using EndpointManager.Interfaces;
using EndpointManager.Model;
using EndpointManager.Services;
using System;
using System.Collections.Generic;

namespace EndpointManager
{
    internal class Program
    {
        static List<Endpoint> endpoints = new List<Endpoint>();
        static IEndpointService endpointService;
        static IUserInputService userInputService;

        static void Main(string[] args)
        {
            userInputService = ServiceFactory.CreateUserInputService(endpoints);
            endpointService = new EndpointService(endpoints, userInputService);

            bool continueRunning = true;
            while (continueRunning)
            {
                MainMenu();

                if (int.TryParse(Console.ReadLine(), out int option))
                {
                    try
                    {
                        switch (option)
                        {
                            case 1:
                                userInputService.PromptNewEndpoint();
                                break;
                            case 2:
                                userInputService.PromptEditEndpoint();
                                break;
                            case 3:
                                userInputService.PromptDeleteEndpoint();
                                break;
                            case 4:
                                endpointService.ListAllEndpoints();
                                break;
                            case 5:
                                endpointService.FindEndpoint();
                                break;
                            case 6:
                                Console.WriteLine("Do you want to exit? (y/n)");
                                string userInput = Console.ReadLine();
                                if (userInput != null && string.Equals(userInput, "y", StringComparison.OrdinalIgnoreCase))
                                {
                                    continueRunning = false;
                                }
                                break;
                            default:
                                userInputService.DisplayMessage("Invalid option, please try again.");
                                break;
                        }
                    }
                    catch (DuplicateSerialNumberException ex)
                    {
                        userInputService.DisplayMessage($"Duplicate serial number error: {ex.Message}");
                    }
                    catch (EndpointNotFoundException ex)
                    {
                        userInputService.DisplayMessage($"Endpoint not found: {ex.Message}");
                    }
                    catch (InvalidValueException ex)
                    {
                        userInputService.DisplayMessage($"Invalid enum value: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        userInputService.DisplayMessage($"An unexpected error occurred: {ex.Message}");
                    }
                }
                else
                {
                    userInputService.DisplayMessage("Invalid input, please enter a number.");
                }
            }
        }

        private static void MainMenu()
        {
            Console.WriteLine("Please select an option from the menu below:");
            Console.WriteLine();
            Console.WriteLine("1) Insert a new endpoint");
            Console.WriteLine("2) Edit an existing endpoint");
            Console.WriteLine("3) Delete an existing endpoint");
            Console.WriteLine("4) List all endpoints");
            Console.WriteLine("5) Find an endpoint by \"Endpoint Serial Number\"");
            Console.WriteLine("6) Exit");
            Console.WriteLine();
            Console.Write("Enter your choice: ");
        }
    }
}
