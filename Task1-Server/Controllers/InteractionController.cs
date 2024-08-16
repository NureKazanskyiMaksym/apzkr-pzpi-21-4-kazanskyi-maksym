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
    public class InteractionController : ControllerBase
    {
        private readonly DbApp _context;

        public InteractionController(DbApp context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> GetInteractions()
        {
            var intercations = await _context.Interactions.ToListAsync();
            var response = new InteractionResponse(intercations);

            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> GetInteraction(int id)
        {
            var interaction = await _context.Interactions.FindAsync(id);

            if (interaction == null)
            {
                return NotFound(new BaseResponse("Interraction not found"));
            }

            var response = new InteractionResponse(interaction);

            return Ok(response);
        }

        [HttpGet("user/{user_id}")]
        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> GetUserInteractions(int user_id)
        {
            var interactions = await _context.Interactions
                .Include(x => x.Access)
                .ThenInclude(x => x.ReceiverAccount)
                .Where(x => x.Access.ReceiverAccount.AccountID == user_id)
                .ToListAsync();

            var response = new InteractionResponse(interactions);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> NewInteraction([FromBody] InteractionRequest interactionRequest)
        {
            var interaction = new Models.Interaction
            {
                Timestamp = DateTime.UtcNow,
                Token = interactionRequest.Token
            };

            var macAddress = string.Join(':', interactionRequest.MACAddress.Select(x => x.ToString("X2")));

            var accessDevice = await _context.AccessDevices
                .FirstOrDefaultAsync(x => x.MacAddress == macAddress);

            if (accessDevice == null)
            {
                interaction.Result = "Device not found";

                _context.Interactions.Add(interaction);
                await _context.SaveChangesAsync();

                return NotFound(new BaseResponse("Device not found"));
            }
            
            var accessToken = await _context.AccessTokens
                .FirstOrDefaultAsync(x => x.Token == interactionRequest.Token);

            if (accessToken == null)
            {
                return NotFound(new BaseResponse("Token not found"));
            }

            if (accessToken == null)
            {
                interaction.Result = "Token not found";

                _context.Interactions.Add(interaction);
                await _context.SaveChangesAsync();

                return NotFound(new BaseResponse("Token not found"));
            }

            var access = await _context.Accesses
                .Include(x => x.AccessDevice)
                .Where(x => x.ReceiverAccountID == accessToken.AccountID && x.ExpiresOn > DateTime.UtcNow)
                .FirstOrDefaultAsync(x => x.AccessDevice.MacAddress == macAddress);

            if (access == null)
            {
                interaction.Result = "Access not found";

                _context.Interactions.Add(interaction);
                await _context.SaveChangesAsync();

                return NotFound(new BaseResponse("Access not found"));
            }

            interaction.AccessID = access.AccessID;

            if (access.ExpiresOn < DateTime.UtcNow)
            {
                interaction.Result = "Access expired";

                _context.Interactions.Add(interaction);
                await _context.SaveChangesAsync();

                return NotFound(new BaseResponse("Access expired"));
            }

            interaction.Result = "Access granted";

            _context.Interactions.Add(interaction);
            await _context.SaveChangesAsync();

            return Ok(new BaseResponse());
        }
    }
}
