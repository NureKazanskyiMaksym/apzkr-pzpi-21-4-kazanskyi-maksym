using EquipmentWatcher.Requests;
using EquipmentWatcher.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EquipmentWatcher.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly DbApp _dbContext;

        public PermissionController(DbApp dbApp)
        {
            _dbContext = dbApp;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Secretary")]
        public IActionResult GetPermission(int id)
        {
            var account = _dbContext.Accounts
                .Include(x => x.Permissions)
                .FirstOrDefault(a => a.AccountID == id);

            if (account == null)
            {
                return NotFound(new BaseResponse("Account not found"));
            }

            var permission = account.Permissions;

            if (!permission.Any())
            {
                return NotFound(new BaseResponse("Permissions not found"));
            }

            var response = new PermissionResponse(permission);
            return Ok(response);
        }

        [HttpGet("own")]
        [Authorize(Roles = "User")]
        public IActionResult GetSelfPermissions()
        {
            var account = _dbContext.Accounts
                .Include(x => x.Permissions)
                .FirstOrDefault(a => a.Login == User.Identity.Name);

            if (account == null)
            {
                return NotFound(new BaseResponse("Account not found"));
            }

            var permission = new PermissionResponse(account.Permissions);
            return Ok(permission);
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult CreatePermission(int id, [FromBody] PermissionRequest permissionRequest)
        {
            var account = _dbContext.Accounts
                .Include(x => x.Permissions)
                .FirstOrDefault(a => a.AccountID == id);

            if (account == null)
            {
                return NotFound(new BaseResponse("Account not found"));
            }

            var permission = _dbContext.Permissions.FirstOrDefault(x => x.Value == permissionRequest.Value);

            if (permission != null)
            {
                return BadRequest(new BaseResponse("Permission already exists"));
            }

            permission = new Models.Permission()
            {
                AccountID = account.AccountID,
                Value = permissionRequest.Value,
            };

            _dbContext.Permissions.Add(permission);
            _dbContext.SaveChanges();

            return Ok(new BaseResponse());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeletePermissions(int id, [FromBody] PermissionRequest permissionRequest)
        {
            var account = _dbContext.Accounts
                .Include(x => x.Permissions)
                .FirstOrDefault(a => a.AccountID == id);

            if (account == null)
            {
                return NotFound(new BaseResponse("Account not found"));
            }

            var permission = _dbContext.Permissions.FirstOrDefault(x => x.Value == permissionRequest.Value);

            if (permission == null)
            {
                return NotFound(new BaseResponse("Permission not found"));
            }

            _dbContext.Permissions.Remove(permission);
            _dbContext.SaveChanges();

            return Ok(new BaseResponse());
        }
    }
}
