using FleetManagement.Core.Enumerations;

namespace FleetManagement.Core.DTOs.Input
{
    public class DeliveryPointDto
    {
        public DeliveryPointTypes type { get; set; }
        public int value { get; set; }
    }
}
