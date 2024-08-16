using System.ComponentModel.DataAnnotations;

namespace EquipmentWatcher.Requests
{
	public class PersonRequest
	{
		[Required]
		public string FirstName { get; set; }

		public string Midname { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		public string Email { get; set; }
	}
}
