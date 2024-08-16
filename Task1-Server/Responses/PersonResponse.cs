using EquipmentWatcher.Models;

namespace EquipmentWatcher.Responses
{
	public class PersonResponse : BaseResponse
	{
		public PersonResponse(Person person) : base(true, null, string.Empty)
		{
			Result = new View(person);
		}

		public PersonResponse(IEnumerable<Person> persons) : base(true, null, string.Empty)
		{
			Result = persons.Select(p => new View(p));
		}

		public class View
		{
			public int ID { get; set; }
			public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
			public string Email { get; set; }

			public View(Person person)
			{
				this.ID = person.PersonID;
                this.FirstName = person.FirstName;
                this.MiddleName = person.MiddleName;
                this.LastName = person.LastName;
				this.Email = person.Email;
			}
		}
	}
}
