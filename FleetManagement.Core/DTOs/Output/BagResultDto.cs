using FleetManagement.Core.Enumerations;

namespace FleetManagement.Core.DTOs.Output
{
    public class BagResultDto
    {
        public int id { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime? updateDate { get; set; }
        public string barcode { get; set; } = string.Empty;
        public int deliveryPointId { get; set; }
        public BagStatuses state { get; set; }
    }
}
