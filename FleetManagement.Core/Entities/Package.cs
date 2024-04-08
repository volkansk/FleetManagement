using FleetManagement.Core.Enumerations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetManagement.Core.Entities
{
    public class Package : BaseEntity
    {
        public string Barcode { get; set; } = string.Empty;
        public int DeliveryPointId { get; set; }
        public int VolumetricWeight { get; set; }
        public PackageStatuses State { get; set; }
        public int? BagId { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [NotMapped]
        public virtual DeliveryPoint? DeliveryPoint { get; set; }
        [NotMapped]
        public virtual Bag? Bag { get; set; }

        public Package ChangeState(PackageStatuses packageStatus)
        {
            this.State = packageStatus;
            return this;
        }
    }
}
