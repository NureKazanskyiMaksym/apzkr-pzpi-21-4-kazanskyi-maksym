using EquipmentWatcher.Models;

namespace EquipmentWatcher.Responses
{
	public class AccountResponse : BaseResponse
	{
		public AccountResponse(Account account) : base(true, null, string.Empty)
		{
			Result = new View(account);
		}

		public AccountResponse(IEnumerable<Account> accounts) : base(true, null, string.Empty)
		{
			Result = accounts.Select(a => new View(a));
		}

		public class View
		{
			public int ID { get; set; }
			public string Login { get; set; }
			public DateTime LastSession { get; set; }

			public View(Account account)
			{
				this.ID = account.AccountID;
				this.Login = account.Login;
				this.LastSession = account.LastSession;
			}
		}
	}
}
