using EndpointManager.Model;

namespace EndpointManager.Interfaces
{
    public interface IEndpointService
    {
        void InsertEndpoint(Endpoint endpoint);
        void EditEndpoint(Endpoint endpoint, States newState);
        void DeleteEndpoint(Endpoint endpoint);
        void ListAllEndpoints();
        void FindEndpoint();
        Endpoint FindBySerialNumber(string serialNumber);
    }
}
