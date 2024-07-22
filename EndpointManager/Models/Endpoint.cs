using EndpointManager.Enums;

namespace EndpointManager.Model
{
    public class Endpoint
    {
        public string SerialNumber { get; set; }
        public Models MeterModelId { get; set; }
        public int MeterNumber { get; set; }
        public string MeterFirmwareVersion { get; set; }
        public States SwitchState { get; set; }

    }
}
