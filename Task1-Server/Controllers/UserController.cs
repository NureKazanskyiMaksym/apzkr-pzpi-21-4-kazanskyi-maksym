using EquipmentWatcher.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EquipmentWatcher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication", Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly DbApp _dbContext;

        public UserController(DbApp dbApp)
        {
            _dbContext = dbApp;
        }

        [HttpGet("me")]
        public IActionResult GetMe()
        {
            var account = _dbContext.Accounts
                .Include(x => x.Person)
                .FirstOrDefault(p => p.Login == User.Identity.Name);

            if (account == null)
            {
                return NotFound(new BaseResponse("Account not found"));
            }

            account.LastSession = DateTime.UtcNow;
            _dbContext.SaveChanges();

            var response = new PersonAccountResponse(account.Person, account);
            return Ok(response);
        }
    }
}
