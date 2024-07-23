using EndpointManager.Enums;
using EndpointManager.Interfaces;
using EndpointManager.Model;
using EndpointManager.Services;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace EndpointManager.Tests
{
    public class EndpointServiceTests
    {
        private static IUserInputService _userInputService;
        private readonly List<Endpoint> _endpoints;
        private readonly EndpointService _endpointService;
        private readonly Endpoint _initialEndpoint;

        // Constructor injection for dependencies
        public EndpointServiceTests()
        {
            _endpoints = new List<Endpoint>();
            //_mockUserInputService = new Mock<IUserInputService>();
            _userInputService = ServiceFactory.CreateUserInputService(_endpoints);
            _initialEndpoint = new Endpoint
            {
                SerialNumber = "12345",
                MeterModelId = Models.NSX1P2W,
                MeterNumber = 100,
                MeterFirmwareVersion = "1.0",
                SwitchState = States.Connected
            };
            _endpointService = new EndpointService(_endpoints, _userInputService);
        }


        [Fact]
        public void InsertEndpoint_ShouldAddEndpoint_WhenInputsAreValid()
        {

            //Act
            _endpointService.InsertEndpoint(_initialEndpoint);

            // Assert
            Assert.Single(_endpoints);
            var endpoint = _endpoints[0];
            Assert.Equal("12345", endpoint.SerialNumber);
            Assert.Equal(Models.NSX1P2W, endpoint.MeterModelId);
            Assert.Equal(100, endpoint.MeterNumber);
            Assert.Equal("1.0", endpoint.MeterFirmwareVersion);
            Assert.Equal(States.Connected, endpoint.SwitchState);
        }

        [Fact]
        public void EditEndpoint_ShouldUpdateSwitchState_WhenEndpointExists()
        {
            // Arrange
            var endpoint = new Endpoint
            {
                SerialNumber = "123",
                SwitchState = States.Disconnected
            };
            _endpoints.Add(endpoint);


            // Act
            _endpointService.EditEndpoint(endpoint, States.Armed);

            // Assert
            Assert.Equal(States.Armed, endpoint.SwitchState);
        }


        [Fact]
        public void DeleteEndpoint_ShouldRemoveEndpoint_WhenConfirmed()
        {
            // Arrange
            var endpoint = new Endpoint { SerialNumber = "123" };
            _endpoints.Add(endpoint);


            // Act
            _endpointService.DeleteEndpoint(endpoint);

            // Assert
            Assert.Empty(_endpoints);
        }


        [Fact]
        public void ListAllEndpoints_NoEndpoints_DisplaysNoEndpointsMessage()
        {
            // Arrange
            _endpoints.Add(new Endpoint { SerialNumber = "123" });
            var mockUserInputService = new Mock<UserInputService>();
            var endpoints = new List<Endpoint>();
            var endpointService = new EndpointService(endpoints, mockUserInputService.Object);

            // Act
            endpointService.ListAllEndpoints();

            // Assert
            mockUserInputService.Verify(service => service.DisplayMessage("There are no endpoints so far."), Times.Once);
        }

        [Fact]
        public void ListAllEndpoints_WithEndpoints_DisplaysEndpoints()
        {
            // Arrange
            var mockUserInputService = new Mock<UserInputService>();
            var endpoints = new List<Endpoint>
        {
            new Endpoint { SerialNumber = "123" },
            new Endpoint { SerialNumber = "124" }
        };
            var endpointService = new EndpointService(endpoints, mockUserInputService.Object);

            // Act
            endpointService.ListAllEndpoints();

            // Assert
            mockUserInputService.Verify(service => service.DisplayMessage("Here are the endpoints:"), Times.Once);
            mockUserInputService.Verify(service => service.DisplayMessage("--------------------------"), Times.Exactly(endpoints.Count));
        }
    }
}
