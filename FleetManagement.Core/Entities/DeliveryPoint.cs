using FleetManagement.Core.Enumerations;

namespace FleetManagement.Core.Entities
{
    public class DeliveryPoint : BaseEntity
    {
        public DeliveryPoint()
        {
            Bags = new HashSet<Bag>();
            Packages = new HashSet<Package>();
            FleetTransactions = new HashSet<FleetTransaction>();
        }

        public DeliveryPointTypes Type { get; set; }
        public int Value { get; set; }

        public virtual ISet<Bag> Bags { get; set; }
        public virtual ISet<Package> Packages { get; set; }
        public virtual ISet<FleetTransaction> FleetTransactions { get; set; }
    }
}
