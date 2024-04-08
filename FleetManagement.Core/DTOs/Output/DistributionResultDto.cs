namespace FleetManagement.Core.DTOs.Output
{
    public class DistributionResultDto
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
                public int state { get; set; }

                public Delivery ChangeState(int state)
                {
                    this.state = state;
                    return this;
                }
            }
        }
    }
}
