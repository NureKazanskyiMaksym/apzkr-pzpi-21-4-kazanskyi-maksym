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
    public class AccessTokenController : ControllerBase
    {
        private readonly DbApp _dbContext;

        public AccessTokenController(DbApp dbApp)
        {
            _dbContext = dbApp;
        }

        [HttpGet]
        [Authorize(Roles = "Secretary")]
        public IActionResult GetAccessTokens()
        {
            var accessTokens = _dbContext.AccessTokens.ToList();
            var response = new AccessTokenResponse(accessTokens);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Secretary")]
        public IActionResult GetAccessToken(int id)
        {
            var accessToken = _dbContext.AccessTokens.FirstOrDefault(a => a.AccessTokenID == id);
            if (accessToken == null)
            {
                return NotFound(new BaseResponse("Access token not found"));
            }

            var response = new AccessTokenResponse(accessToken);
            return Ok(response);
        }

        [HttpGet("own")]
        public IActionResult GetSelfAccessToken()
        {
            var account = _dbContext.Accounts.FirstOrDefault(x => x.Login == User.Identity.Name);
            if (account == null)
            {
                return NotFound(new BaseResponse("Account not found"));
            }

            var accessToken = _dbContext.AccessTokens.FirstOrDefault(x => x.AccountID == account.AccountID && x.ExpiresOn > DateTime.UtcNow);
            if (accessToken == null)
            {
                return NotFound(new BaseResponse("Access token not found"));
            }

            var response = new AccessTokenResponse(accessToken);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddAccessToken([FromBody] AccessTokenRequest accessTokenRequest)
        {
            if (!_dbContext.Accounts.Any(x => x.AccountID == accessTokenRequest.AccountID))
            {
                return NotFound(new BaseResponse("Account not found"));
            }

            if (_dbContext.AccessTokens.Any(x => x.AccountID == accessTokenRequest.AccountID))
            {
                return BadRequest(new BaseResponse("Access token for this account already exists"));
            }

            var newAccessToken = new AccessToken
            {
                Token = Guid.NewGuid().ToString(),
                AccountID = accessTokenRequest.AccountID,
                ExpiresOn = accessTokenRequest.ExpiresOn
            };

            _dbContext.AccessTokens.Add(newAccessToken);
            _dbContext.SaveChanges();
            return Ok(new BaseResponse());
        }

        [HttpPut]
        public IActionResult UpdateSelfAccessToken(DateTime dateTime)
        {
            var account = _dbContext.Accounts.FirstOrDefault(x => x.Login == User.Identity.Name);
            if (account == null)
            {
                return NotFound(new BaseResponse("Account not found"));
            }

            var accessTokenToUpdate = _dbContext.AccessTokens.FirstOrDefault(x => x.AccountID == account.AccountID);
            if (accessTokenToUpdate == null)
            {
                accessTokenToUpdate = new AccessToken
                {
                    Token = Guid.NewGuid().ToString(),
                    AccountID = account.AccountID,
                    ExpiresOn = DateTime.UtcNow.AddMinutes(20)
                };
                _dbContext.AccessTokens.Add(accessTokenToUpdate);
            }
            else
            {
                var maxPossible = DateTime.UtcNow.AddDays(7);

                accessTokenToUpdate.Token = Guid.NewGuid().ToString();
                accessTokenToUpdate.ExpiresOn = maxPossible > dateTime ? dateTime : maxPossible;
            }
            _dbContext.SaveChanges();
            return Ok(new AccessTokenResponse(accessTokenToUpdate));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult UpdateAccessToken(int id, DateTime dateTime)
        {
            var accessTokenToUpdate = _dbContext.AccessTokens.FirstOrDefault(x => x.AccessTokenID == id);
            if (accessTokenToUpdate == null)
            {
                return NotFound(new BaseResponse("Access token not found"));
            }

            var maxPossible = DateTime.UtcNow.AddDays(7);

            accessTokenToUpdate.Token = Guid.NewGuid().ToString();
            accessTokenToUpdate.ExpiresOn = maxPossible > dateTime ? dateTime : maxPossible;

            _dbContext.SaveChanges();
            return Ok(new BaseResponse());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteAccessToken(int id)
        {
            var accessToken = _dbContext.AccessTokens.FirstOrDefault(x => x.AccessTokenID == id);
            if (accessToken == null)
            {
                return NotFound(new BaseResponse("Access token not found"));
            }

            _dbContext.AccessTokens.Remove(accessToken);
            _dbContext.SaveChanges();
            return Ok(new BaseResponse());
        }
    }
}
