using EndpointManager.Model;
using System.Collections.Generic;

namespace EndpointManager.Services
{
    public class ServiceFactory
    {
        public static UserInputService CreateUserInputService(List<Endpoint> endpoints)
        {
            var userInputService = new UserInputService();
            var endpointService = new EndpointService(endpoints, userInputService);
            userInputService.SetEndpointService(endpointService);
            return userInputService;
        }
    }
}
