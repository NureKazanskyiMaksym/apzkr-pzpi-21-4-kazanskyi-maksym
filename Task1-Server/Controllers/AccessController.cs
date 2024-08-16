using EquipmentWatcher.Requests;
using EquipmentWatcher.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EquipmentWatcher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class AccessController : ControllerBase
    {
        private readonly DbApp _dbContext;

        public AccessController(DbApp dbApp)
        {
            _dbContext = dbApp;
        }

        [HttpGet]
        [Authorize(Roles = "Secretary")]
        public IActionResult GetAccesses()
        {
            var accesses = _dbContext.Accesses.ToList();
            var response = new AccessResponse(accesses);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Secretary")]
        public IActionResult GetAccess(int id)
        {
            var access = _dbContext.Accesses.FirstOrDefault(a => a.AccessID == id);
            if (access == null)
            {
                return NotFound(new BaseResponse("Access not found"));
            }

            var response = new AccessResponse(access);
            return Ok(response);
        }

        [HttpGet("own")]
        public IActionResult GetOwnAccesses()
        {
            var account = _dbContext.Accounts
                .Include(x => x.Person)
                .FirstOrDefault(p => p.Login == User.Identity.Name);

            var accesses = _dbContext.Accesses
                .Include(x => x.ProviderAccount)
                .Include(x => x.ReceiverAccount)
                .Include(x => x.AccessDevice)
                .Where(x => x.ReceiverAccountID == account.AccountID && x.ExpiresOn > DateTime.UtcNow)
                .ToList();

            var response = new AccessResponse(accesses);
            return Ok(response);
        }

        [HttpGet("account/{id}")]
        public IActionResult GetAccountAccesses(int id)
        {
            var account = _dbContext.Accounts.FirstOrDefault(a => a.AccountID == id);
            if (account == null)
            {
                return NotFound(new BaseResponse("Account not found"));
            }

            var accesses = _dbContext.Accesses
                .Include(x => x.ProviderAccount)
                .Include(x => x.ReceiverAccount)
                .Include(x => x.AccessDevice)
                .Where(x => x.ReceiverAccountID == account.AccountID)
                .ToList();

            var response = new AccessResponse(accesses);
            return Ok(response);
        }

        [HttpPost]
        public IActionResult AddAccess([FromBody] AccessRequest accessRequest)
        {
            var account = _dbContext.Accounts.FirstOrDefault(x => x.Login == User.Identity.Name);

            if (_dbContext.Accesses.Any(x => x.ProviderAccountID == account.AccountID
                && x.ReceiverAccountID == accessRequest.ReceiverAccountID
                && x.AccessDeviceID == accessRequest.AccessDeviceID
                && x.ExpiresOn > DateTime.UtcNow))
            {
                return NotFound(new BaseResponse("Access with given parameters already exists"));
            }

            if (!_dbContext.Accounts.Any(x => x.AccountID == account.AccountID))
            {
                return NotFound(new BaseResponse("Provider account not found"));
            }

            if (!_dbContext.Accounts.Any(x => x.AccountID == accessRequest.ReceiverAccountID))
            {
                return NotFound(new BaseResponse("Receiver account not found"));
            }

            if (!_dbContext.AccessDevices.Any(x => x.AccessDeviceID == accessRequest.AccessDeviceID))
            {
                return NotFound(new BaseResponse("Access device not found"));
            }

            var newAccess = new Models.Access
            {
                ProviderAccountID = account.AccountID,
                ReceiverAccountID = accessRequest.ReceiverAccountID,
                AccessDeviceID = accessRequest.AccessDeviceID,
                CreatedAt = DateTime.UtcNow,
                ExpiresOn = accessRequest.ExpiresOn,
                AllowProvide = accessRequest.AllowProvide
            };

            _dbContext.Accesses.Add(newAccess);
            _dbContext.SaveChanges();
            return Ok(new BaseResponse());
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAccess(int id, [FromBody] AccessRequest accessRequest)
        {
            var account = _dbContext.Accounts.FirstOrDefault(x => x.Login == User.Identity.Name);

            if (account == null)
            {
                return NotFound(new BaseResponse("Authorized account not found"));
            }

            var token = _dbContext.AccessTokens.FirstOrDefault(a => a.AccountID == account.AccountID);

            if (token == null || token.ExpiresOn < DateTime.UtcNow)
            {
                return NotFound(new BaseResponse("Access token is invalid"));
            }

            var access = _dbContext.Accesses.FirstOrDefault(a => a.AccessID == id);

            if (access == null)
            {
                return NotFound(new BaseResponse("Access not found"));
            }

            if (access.ProviderAccountID != account.AccountID)
            {
                return NotFound(new BaseResponse("You aren't allowed to change this access"));
            }

            if (!_dbContext.Accounts.Any(x => x.AccountID == account.AccountID))
            {
                return NotFound(new BaseResponse("Provider account not found"));
            }

            if (!_dbContext.Accounts.Any(x => x.AccountID == accessRequest.ReceiverAccountID))
            {
                return NotFound(new BaseResponse("Receiver account not found"));
            }

            if (!_dbContext.AccessDevices.Any(x => x.AccessDeviceID == accessRequest.AccessDeviceID))
            {
                return NotFound(new BaseResponse("Access device not found"));
            }

            access.ProviderAccountID = account.AccountID;
            access.ReceiverAccountID = accessRequest.ReceiverAccountID;
            access.AccessDeviceID = accessRequest.AccessDeviceID;
            access.ExpiresOn = accessRequest.ExpiresOn;
            access.AllowProvide = accessRequest.AllowProvide;

            _dbContext.SaveChanges();
            return Ok(new BaseResponse());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administartor")]
        public IActionResult DeleteAccess(int id)
        {   
            var access = _dbContext.Accesses.FirstOrDefault(a => a.AccessID == id);

            if (access == null)
            {
                return NotFound(new BaseResponse("Access not found"));
            }

            _dbContext.Accesses.Remove(access);
            _dbContext.SaveChanges();
            return Ok(new BaseResponse());
        }

        [HttpDelete("cancel/{id}")]
        public IActionResult CancelAccess(int id)
        {
            var account = _dbContext.Accounts.FirstOrDefault(x => x.Login == User.Identity.Name);
            var access = _dbContext.Accesses.FirstOrDefault(a => a.AccessID == id && a.ProviderAccountID == account.AccountID);

            if (access == null)
            {
                return NotFound(new BaseResponse("Access not found"));
            }

            _dbContext.Accesses.Remove(access);
            _dbContext.SaveChanges();
            return Ok(new BaseResponse());
        }
    }
}
