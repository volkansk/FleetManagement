namespace FleetManagement.Core.DTOs.Output
{
    public class VehicleResultDto
    {
        public int id { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime? updateDate { get; set; }
        public string plate { get; set; } = string.Empty;
    }
}
