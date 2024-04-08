namespace FleetManagement.Core.DTOs.Input
{
    public class DistributionCommandDto
    {
        public string plate { get; set; } = string.Empty;
        public List<Route> route { get; set; } = new List<Route>();

        public class Route
        {
            public int deliveryPoint { get; set; }
            public List<Delivery> deliveries { get; set; } = new List<Delivery>();

            public class Delivery
            {
                public string barcode { get; set; } = string.Empty;
            }
        }
    }
}
