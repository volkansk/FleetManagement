namespace FleetManagement.Core.DTOs.Output
{
    public class PackageResultDto
    {
        public int id { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime? updateDate { get; set; }
        public string barcode { get; set; } = string.Empty;
        public int deliveryPointId { get; set; }
        public int volumetricWeight { get; set; }
    }
}
