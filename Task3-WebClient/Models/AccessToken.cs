using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace EquipmentWatcher.Models
{
    public class AccessToken
    {
		[Key]
        public int AccessTokenID { get; set; }

        [Required]
        public int AccountID { get; set; }

        [Required]
        public string Token { get; set; }

        public DateTime ExpiresOn { get; set; }

		[ValidateNever]
        public virtual Account Account { get; set; }
    }
}
