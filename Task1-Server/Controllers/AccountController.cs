using EquipmentWatcher.Models;
using EquipmentWatcher.Requests;
using EquipmentWatcher.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentWatcher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class AccountController : ControllerBase
    {
        private readonly DbApp _dbContext;

        public AccountController(DbApp dbApp)
        {
            _dbContext = dbApp;
        }

        [HttpGet]
        [Authorize(Roles = "Secretary")]
        public IActionResult GetAccounts()
        {
            var accounts = _dbContext.Accounts.ToList();
            var response = new AccountResponse(accounts);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Secretary")]
        public IActionResult GetAccount(int id)
        {
            var account = _dbContext.Accounts.FirstOrDefault(a => a.AccountID == id);
            if (account == null)
            {
                return NotFound(new BaseResponse("Account not found"));
            }

            var response = new AccountResponse(account);
            return Ok(response);
        }

        [HttpGet("person/{id}")]
        public IActionResult GetAccountByPerson(int id)
        {
            var account = _dbContext.Accounts.FirstOrDefault(a => a.PersonID == id);
            if (account == null)
            {
                return NotFound(new BaseResponse("Account not found"));
            }

            var response = new AccountResponse(account);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddAccount([FromBody] AccountRequest accountRequest)
        {
            if (_dbContext.Accounts.Any(x => x.PersonID == accountRequest.PersonID))
            {
                return NotFound(new BaseResponse("Person with given ID is already used"));
            }

            if (!_dbContext.Persons.Any(x => x.PersonID == accountRequest.PersonID))
            {
                return NotFound(new BaseResponse("Person with given ID was not found"));
            }

            var newAccount = new Account
            {
                PersonID = accountRequest.PersonID,
                Login = accountRequest.Login,
                Password = accountRequest.Password
            };

            var account_entity = _dbContext.Accounts.Add(newAccount);

            _dbContext.SaveChanges();

            return Ok(new BaseResponse());
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult UpdateAccount(int id, [FromBody] AccountRequest accountRequest)
        {
            var account = _dbContext.Accounts.FirstOrDefault(a => a.AccountID == id);
            if (account == null)
            {
                return NotFound(new BaseResponse("Account not found"));
            }

            if (!_dbContext.Persons.Any(x => x.PersonID == account.PersonID))
            {
                return NotFound(new BaseResponse("Person with given ID was not found"));
            }

            account.PersonID = accountRequest.PersonID;
            account.Login = accountRequest.Login;
            account.Password = accountRequest.Password;

            _dbContext.SaveChanges();

            return Ok(new BaseResponse());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteAccount(int id)
        {
            var account = _dbContext.Accounts.FirstOrDefault(a => a.AccountID == id);
            if (account == null)
            {
                return NotFound(new BaseResponse("Account not found"));
            }

            _dbContext.Accounts.Remove(account);
            _dbContext.SaveChanges();

            return Ok(new BaseResponse());
        }
    }
}
