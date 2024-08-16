using EquipmentWatcher.Models;

namespace EquipmentWatcher.Responses
{
    public class PersonAccountResponse : BaseResponse
    {
        public PersonAccountResponse(Person person, Account account) : base(true, null, string.Empty)
        {
            Result = new View(person, account);
        }

        public PersonAccountResponse(IEnumerable<Tuple<Person,Account>> persons) : base(true, null, string.Empty)
        {
            Result = persons.Select(p => new View(p.Item1, p.Item2));
        }

        public class View
        {
            public int PersonID { get; set; }
            public int AccountID { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Login { get; set; }

            public View(Person person, Account account)
            {
                this.PersonID = person.PersonID;
                this.AccountID = account.AccountID;
                this.FirstName = person.FirstName;
                this.MiddleName = person.MiddleName;
                this.LastName = person.LastName;
                this.Email = person.Email;
                this.Login = account.Login;
            }
        }
    }
}
