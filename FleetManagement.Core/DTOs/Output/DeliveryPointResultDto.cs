namespace FleetManagement.Core.DTOs.Output
{
    public class DeliveryPointResultDto
    {
        public int id { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int type { get; set; }
        public int value { get; set; }
    }
}
