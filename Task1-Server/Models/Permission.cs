using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace EquipmentWatcher.Models
{
	public class Permission
	{
		[Key]
		public int PermissionID { get; set; }

		public int AccountID { get; set; }

		public string Value { get; set; }

		[ValidateNever]
        public virtual Account Account { get; set; }
    }
}
