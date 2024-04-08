namespace FleetManagement.Service.Constants
{
    public class Messages
    {
        #region NotFound

        public const string BagNotFound = "Bag ({0}) not found.";
        public const string BarcodeNotFound = "Barcode ({0}) not found.";
        public const string DeliveryPointNotFound = "Delivery Point ({0}) not found.";
        public const string PackageNotFound = "Package value ({0}) not found.";
        public const string VehicleNotFound = "Vehicle ({0}) not found.";

        #endregion

        #region AlreadyExist

        public const string BagAlreadyExist = "Bag ({0}) already exist.";
        public const string DeliveryPointAlreadyExist = "Delivery point ({0}) already exist.";
        public const string PackageAlreadyExist = "Package ({0}) already exist.";
        public const string VehicleAlreadyExist = "Vehicle ({0}) already exist.";

        #endregion

        #region FleetManagement

        public const string WrongDeliveryPoint = "Unable to unload because wrong delivery point was entered";
        public const string PackageUnloaded = "Package Unloaded";
        public const string BagUnloaded = "Bag Unloaded";
        public const string PackageIntoBagUnloaded = "Package into Bag Unloaded";
        public const string BagUnloadedAfterPackages = "Bag unloaded after packages inside";

        #endregion

        public const string NotSamePackageBagDeliveryPoint = "Package and Bag delivery point are not same.";

        #region GenericException

        public const string InternalServerError = "Internal Server errors. Check Logs!";

        #endregion
    }
}
