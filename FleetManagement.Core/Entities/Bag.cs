using FleetManagement.Core.Enumerations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetManagement.Core.Entities
{
    public class Bag : BaseEntity
    {
        public Bag()
        {
            Packages = new HashSet<Package>();
        }

        public string Barcode { get; set; } = string.Empty;
        public int DeliveryPointId { get; set; }
        public BagStatuses State { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [NotMapped]
        public virtual DeliveryPoint? DeliveryPoint { get; set; }
        public virtual ISet<Package> Packages { get; set; }

        public Bag ChangeState(BagStatuses bagStatus)
        {
            this.State = bagStatus;
            return this;
        }
    }
}
