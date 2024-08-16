namespace EquipmentWatcher.Requests
{
    public class AccessRequest
    {
        public int ReceiverAccountID { get; set; }

        public int AccessDeviceID { get; set; }

        public DateTime ExpiresOn { get; set; }

        public bool AllowProvide { get; set; }
    }
}
