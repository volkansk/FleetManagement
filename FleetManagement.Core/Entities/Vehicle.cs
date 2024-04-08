namespace FleetManagement.Core.Entities
{
    public class Vehicle : BaseEntity
    {
        public Vehicle()
        {
            FleetTransactions = new HashSet<FleetTransaction>();
        }

        public string Plate { get; set; } = string.Empty;

        public virtual ISet<FleetTransaction> FleetTransactions { get; set; }
    }
}
