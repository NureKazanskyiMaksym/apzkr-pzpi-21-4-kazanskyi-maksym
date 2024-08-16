using System.ComponentModel.DataAnnotations;

namespace EquipmentWatcher.Requests
{
    public class AccessDevicesRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string MACAddress { get; set; }
    }
}
