namespace FleetManagement.Core.DTOs.Input
{
    public class PackageDto
    {
        public string barcode { get; set; } = string.Empty;
        public int deliveryPointValue { get; set; }
        public int volumetricWeight { get; set; }
    }
}
