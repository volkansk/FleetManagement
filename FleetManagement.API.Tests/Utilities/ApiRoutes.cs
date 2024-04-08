namespace FleetManagement.API.Tests.Utilities
{
    public static class ApiRoutes
    {
        public static class Vehicle
        {
            private static readonly string vehicleControllerUrl = "api/vehicles";
            public static readonly string AddSync = vehicleControllerUrl;
        }

        public static class Bag
        {
            private static readonly string bagControllerUrl = "api/bags";
            public static readonly string GetByBarcodeSync = string.Concat(bagControllerUrl, "/{barcode}");
            public static readonly string AddSync = bagControllerUrl;
        }

        public static class DeliveryPoint
        {
            private static readonly string deliveryPointControllerUrl = "api/deliverypoints";
            public static readonly string GetByValueSync = string.Concat(deliveryPointControllerUrl, "/{value}");
            public static readonly string AddSync = deliveryPointControllerUrl;
        }

        public static class Package
        {
            private static readonly string packageControllerUrl = "api/packages";
            public static readonly string GetByBarcodeSync = string.Concat(packageControllerUrl, "/{barcode}");
            public static readonly string AddSync = packageControllerUrl;
            public static readonly string AssignSinglePackageAsync = packageControllerUrl;
        }

        public static class Fleet
        {
            private static readonly string fleetControllerUrl = "api/fleets";
            public static readonly string DistributeAsync = string.Concat(fleetControllerUrl, "/distribute");
        }
    }
}
