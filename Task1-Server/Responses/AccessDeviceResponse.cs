using EquipmentWatcher.Models;
using System.ComponentModel.DataAnnotations;

namespace EquipmentWatcher.Responses
{
    public class AccessDeviceResponse : BaseResponse
    {
        public AccessDeviceResponse(AccessDevice account) : base(true, null, string.Empty)
        {
            Result = new View(account);
        }

        public AccessDeviceResponse(IEnumerable<AccessDevice> accounts) : base(true, null, string.Empty)
        {
            Result = accounts.Select(a => new View(a));
        }

        public class View
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string MACAddress { get; set; }

            public View(AccessDevice device)
            {
                this.ID = device.AccessDeviceID;
                this.Name = device.Name;
                this.Description = device.Description;
                this.MACAddress = device.MacAddress;
            }
        }
    }
}
