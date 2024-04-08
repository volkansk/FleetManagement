using System.ComponentModel.DataAnnotations.Schema;

namespace FleetManagement.Core.Entities
{
    public class FleetTransaction : BaseEntity
    {
        public Guid TransactionId { get; set; }
        public int VehicleId { get; set; }
        public int? DeliveryPointId { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public int? State { get; set; }
        public string? Message { get; set; }

        [NotMapped]
        public virtual Vehicle? Vehicle { get; set; }
        [NotMapped]
        public virtual DeliveryPoint? DeliveryPoint { get; set; }
    }
}
