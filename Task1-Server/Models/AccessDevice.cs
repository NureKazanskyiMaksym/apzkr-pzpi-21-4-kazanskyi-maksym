using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace EquipmentWatcher.Models
{
	public class AccessDevice
	{
		[Key]
		public int AccessDeviceID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string MacAddress { get; set; }

        [ValidateNever]
        public virtual List<Access> Accesses { get; set; }
    }
}
