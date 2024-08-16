using EquipmentWatcher.Models;

namespace EquipmentWatcher.Requests
{
    public class AccessTokenRequest
    {
        public int AccountID { get; set; }

        public DateTime ExpiresOn { get; set; }

        public string Token { get; set; }
    }
}
