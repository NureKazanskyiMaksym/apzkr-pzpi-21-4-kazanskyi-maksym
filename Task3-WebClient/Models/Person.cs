using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquipmentWatcher.Models
{
	public class Person
	{
		[Key]
		public int PersonID { get; set; }

		[Required]
		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required, EmailAddress]
		public string Email { get; set; }

		[ValidateNever]
        [NotMapped]
        public virtual Account Account { get; set; }

		[ValidateNever]
        [NotMapped]
        public virtual List<Access> Accesses { get; set; }
	}
}
