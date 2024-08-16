using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace EquipmentWatcher.Models
{
	public class Access
	{
		[Key]
		public int AccessID { get; set; }

		[Required]
		public int ProviderAccountID { get; set; }

        [Required]
		public int ReceiverAccountID { get; set; }

		[Required]
		public int AccessDeviceID { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }

		[Required]
		public DateTime ExpiresOn { get; set; }

		public bool AllowProvide { get; set; }
		
		[ValidateNever]
        public virtual Account ProviderAccount { get; set; }

		[ValidateNever]
        public virtual Account ReceiverAccount { get; set; }

		[ValidateNever]
        public virtual AccessDevice AccessDevice { get; set; }

		[ValidateNever]
        public virtual List<Interaction> Interactions { get; set; }
    }
}
