using EndpointManager.Enums;
using EndpointManager.Exceptions;
using EndpointManager.Interfaces;
using EndpointManager.Model;
using EndpointManager.Services;
using System.Collections.Generic;
using Xunit;

namespace EndpointManager.Tests
{
    public class EndpointServiceTests
    {
        private static IUserInputService _mockUserInputService;
        private readonly List<Endpoint> _endpoints;
        private readonly EndpointService _endpointService;
        private readonly Endpoint _initialEndpoint;

        // Constructor injection for dependencies
        public EndpointServiceTests()
        {
            _mockUserInputService = new UserInputService();
            _endpoints = new List<Endpoint>();
            _initialEndpoint = new Endpoint
            {
                SerialNumber = "12345",
                MeterModelId = Models.NSX1P2W,
                MeterNumber = 100,
                MeterFirmwareVersion = "1.0",
                SwitchState = States.Connected
            };
            _endpointService = new EndpointService(_endpoints, _mockUserInputService);
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
            _endpointService.EditEndpoint();

            // Assert
            Assert.Equal(States.Armed, endpoint.SwitchState);
        }

        [Fact]
        public void EditEndpoint_ShouldThrowException_WhenEndpointDoesNotExist()
        {
            // Arrange

            // Act & Assert
            var exception = Assert.Throws<EndpointNotFoundException>(() => _endpointService.EditEndpoint());
            Assert.Equal("There is no endpoint with that serial number, please try again.", exception.Message);
        }

        [Fact]
        public void DeleteEndpoint_ShouldRemoveEndpoint_WhenConfirmed()
        {
            // Arrange
            var endpoint = new Endpoint { SerialNumber = "123" };
            _endpoints.Add(endpoint);


            // Act
            _endpointService.DeleteEndpoint();

            // Assert
            Assert.Empty(_endpoints);
        }

        [Fact]
        public void DeleteEndpoint_ShouldNotRemoveEndpoint_WhenNotConfirmed()
        {
            // Arrange
            var endpoint = new Endpoint { SerialNumber = "123" };
            _endpoints.Add(endpoint);


            // Act
            _endpointService.DeleteEndpoint();

            // Assert
            Assert.Single(_endpoints);
        }

        [Fact]
        public void ListAllEndpoints_ShouldDisplayAllEndpoints()
        {
            // Arrange
            _endpoints.Add(new Endpoint { SerialNumber = "123" });


            // Act
            _endpointService.ListAllEndpoints();

            // Assert
        }

        [Fact]
        public void FindEndpoint_ShouldDisplayEndpoint_WhenFound()
        {
            // Arrange
            var endpoint = new Endpoint { SerialNumber = "123" };
            _endpoints.Add(endpoint);

            // Act
            _endpointService.FindEndpoint();

            // Assert
        }

        [Fact]
        public void FindEndpoint_ShouldThrowException_WhenNotFound()
        {
            // Arrange

            // Act & Assert
            var exception = Assert.Throws<EndpointNotFoundException>(() => _endpointService.FindEndpoint());
            Assert.Equal("There is no endpoint with that serial number, please try again.", exception.Message);
        }
    }
}
