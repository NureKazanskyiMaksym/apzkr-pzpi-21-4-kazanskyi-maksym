using EquipmentWatcher.Models;
using System.ComponentModel.DataAnnotations;

namespace EquipmentWatcher.Responses
{
    public class AccessResponse : BaseResponse
    {
        public AccessResponse(Access access) : base(true, null, string.Empty)
        {
            Result = new View(access);
        }

        public AccessResponse(IEnumerable<Access> accesses) : base(true, null, string.Empty)
        {
            Result = accesses.Select(a => new View(a));
        }

        public class View
        {
            public int ID { get; set; }
            public int ProviderAccountID { get; set; }
            public int ReceiverAccountID { get; set; }
            public int AccessDeviceID { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime ExpiresOn { get; set; }
            public bool AllowProvide { get; set; }

            public View(Access device)
            {
                this.ID = device.AccessID;
                this.ProviderAccountID = device.ProviderAccountID;
                this.ReceiverAccountID = device.ReceiverAccountID;
                this.AccessDeviceID = device.AccessDeviceID;
                this.CreatedAt = device.CreatedAt;
                this.ExpiresOn = device.ExpiresOn;
                this.AllowProvide = device.AllowProvide;
            }
        }
    }
}
