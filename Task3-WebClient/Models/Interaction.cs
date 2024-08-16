using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace EquipmentWatcher.Models
{
	public class Interaction
	{
		[Key]
		public int InteractionID { get; set; }

		public int? AccessID { get; set; }

        public string Token { get; set; }

		public DateTime Timestamp { get; set; }

		public string Result { get; set; }

		[ValidateNever]
		public virtual Access Access { get; set; }
    }
}
