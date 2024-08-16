using System.ComponentModel.DataAnnotations;

namespace EquipmentWatcher.Requests
{
	public class AccountRequest
    {
        public int PersonID { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}
