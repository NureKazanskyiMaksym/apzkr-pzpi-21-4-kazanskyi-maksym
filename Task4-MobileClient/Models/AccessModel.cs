using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EquipmentWatcherMAUI.Models
{
    public class AccessModel
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("providerAccountID")]
        public int ProviderAccountID { get; set; }
        [JsonPropertyName("receiverAccountID")]
        public int ReceiverAccountID { get; set; }
        [JsonPropertyName("accessDeviceID")]
        public int AccessDeviceID { get; set; }
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("expiresOn")]
        public DateTime ExpiresOn { get; set; }
        [JsonPropertyName("allowProvide")]
        public bool AllowProvide { get; set; }
    }
}
