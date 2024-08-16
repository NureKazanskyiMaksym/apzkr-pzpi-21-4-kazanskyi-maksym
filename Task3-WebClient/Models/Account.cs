using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace EquipmentWatcher.Models
{
	public class Account
    {
		[Key]
		public int AccountID { get; set; }

		public int PersonID { get; set; }

        [Required]
		public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime LastSession { get; set; }

		[ValidateNever]
		public virtual Person Person { get; set; }

        [ValidateNever]
        public virtual AccessToken? AccessToken { get; set; }

        [ValidateNever]
        public virtual List<Permission> Permissions { get; set; }

		[ValidateNever]
        public virtual List<Access> ProviderAccesses { get; set; }

		[ValidateNever]
        public virtual List<Access> ReceiverAccesses { get; set; }
    }
}
