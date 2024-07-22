using EndpointManager.Model;

namespace EndpointManager.Interfaces
{
    public interface IEndpointService
    {
        void InsertEndpoint(Endpoint endpoint);
        void GetInfoNewEndpoint();
        void EditEndpoint();
        void DeleteEndpoint();
        void ListAllEndpoints();
        void FindEndpoint();
    }
}
