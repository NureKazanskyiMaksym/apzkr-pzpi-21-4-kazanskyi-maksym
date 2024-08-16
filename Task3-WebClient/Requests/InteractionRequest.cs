using System.ComponentModel.DataAnnotations;

namespace EquipmentWatcher.Requests
{
    public class InteractionRequest
    {
        public string Token { get; set; }

        public List<byte> MACAddress { get; set; }
    }
}
