using EquipmentWatcher.Models;

namespace EquipmentWatcher.Responses
{
    public class AccessTokenResponse : BaseResponse
    {
        public AccessTokenResponse(AccessToken token) : base(true, null, string.Empty)
        {
            Result = new View(token);
        }

        public AccessTokenResponse(IEnumerable<AccessToken> tokens) : base(true, null, string.Empty)
        {
            Result = tokens.Select(t => new View(t));
        }

        public class View
        {
            public int ID { get; set; }
            public string Token { get; set; }
            public int AccountID { get; set; }
            public DateTime ExpiresOn { get; set; }

            public View(AccessToken token)
            {
                this.ID = token.AccessTokenID;
                this.Token = token.Token;
                this.AccountID = token.AccountID;
                this.ExpiresOn = token.ExpiresOn;
            }
        }
    }
}
