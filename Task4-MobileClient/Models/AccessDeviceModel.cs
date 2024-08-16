using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EquipmentWatcherMAUI.Models
{
    public class AccessDeviceModel
    {
        public AccessDeviceModel()
        {
        }

        public AccessDeviceModel(AccessModel access, DeviceModel device)
        {
            AccessID = access.ID;
            DeviceID = device.ID;
            Name = device.Name;
            Description = device.Description;
            MACAddress = device.MACAddress;
            ProviderAccountID = access.ProviderAccountID;
            ReceiverAccountID = access.ReceiverAccountID;
            AccessDeviceID = access.AccessDeviceID;
            CreatedAt = access.CreatedAt;
            ExpiresOn = access.ExpiresOn;
            AllowProvide = access.AllowProvide;
        }

        [JsonPropertyName("accessID")]
        public int AccessID { get; set; }

        [JsonPropertyName("deviceID")]
        public int DeviceID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("macAddress")]
        public string MACAddress { get; set; }

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

        [JsonPropertyName("isReceiverProvider")]
        public bool AllowProvide { get; set; }
    }
}
