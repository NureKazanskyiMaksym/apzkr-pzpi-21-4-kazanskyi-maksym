using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EquipmentWatcherMAUI.Models
{
    public class AccessTokenModel
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("accountID")]
        public int AccountID { get; set; }
        [JsonPropertyName("expiresOn")]
        public DateTime ExpiresOn { get; set; }
    }
}
