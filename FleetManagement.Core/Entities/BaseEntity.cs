namespace FleetManagement.Core.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            CreatedDate = DateTime.Now;
        }

        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
