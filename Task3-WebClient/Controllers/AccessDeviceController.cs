using EquipmentWatcher.Models;
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
    public class AccessDeviceController : ControllerBase
    {
        private readonly DbApp _dbContext;

        public AccessDeviceController(DbApp dbApp)
        {
            _dbContext = dbApp;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,Secretary")]
        public IActionResult GetDevices()
        {
            var devices = _dbContext.AccessDevices.ToList();
            var reponse = new AccessDeviceResponse(devices);
            return Ok(reponse);
        }

        [HttpGet("available")]
        public IActionResult GetAviableDevices()
        {
            var account = _dbContext.Accounts
                .Include(x => x.Person)
                .FirstOrDefault(p => p.Login == User.Identity.Name);

            if (account == null)
            {
                return NotFound(new BaseResponse("Account not found"));
            }

            var devices = _dbContext.Accesses
                .Include(x => x.AccessDevice)
                .Where(x => x.ReceiverAccountID == account.AccountID)
                .Select(x => x.AccessDevice)
                .ToList();

            var response = new AccessDeviceResponse(devices);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetDevice(int id)
        {
            var device = _dbContext.AccessDevices.FirstOrDefault(x => x.AccessDeviceID == id);
            if (device == null)
            {
                return NotFound(new BaseResponse("Device not found"));
            }
            var response = new AccessDeviceResponse(device);
            return Ok(response);
        }

        [HttpGet("{id}/members")]
        public IActionResult GetMembers(int id)
        {
            var device = _dbContext.AccessDevices.FirstOrDefault(x => x.AccessDeviceID == id);
            if (device == null)
            {
                return NotFound(new BaseResponse("Device not found"));
            }

            var members = _dbContext.AccessDevices
                .Include(x => x.Accesses)
                .ThenInclude(x => x.ReceiverAccount)
                .ThenInclude(x => x.Person)
                .Where(x => x.AccessDeviceID == id)
                .SelectMany(
                    x => x.Accesses
                    .Where(x => x.ExpiresOn > DateTime.UtcNow)
                    .Select(y => new Tuple<Person, Account>(y.ReceiverAccount.Person, y.ReceiverAccount)))
                .AsEnumerable();

            var response = new PersonAccountResponse(members);
            return Ok(response);
        }

        [HttpGet("{id}/grant/search/")]
        public IActionResult SearchMembers(int id, [FromQuery(Name = "q")] string query = "")
        {
            var account = _dbContext.Accounts.FirstOrDefault(x => x.Login == User.Identity.Name);
            var device = _dbContext.AccessDevices.FirstOrDefault(x => x.AccessDeviceID == id);
            if (device == null)
            {
                return NotFound(new BaseResponse("Device not found"));
            }

            var accessDeviceMembers = _dbContext.AccessDevices
                .Include(x => x.Accesses)
                .ThenInclude(x => x.ReceiverAccount)
                .ThenInclude(x => x.Person)
                .Where(x => x.AccessDeviceID == id)
                .SelectMany(x => x.Accesses
                    .Where(y => y.ExpiresOn > DateTime.UtcNow)
                    .Select(y => y.ReceiverAccount.Person));

            var personMembers = _dbContext.Persons
                .Except(accessDeviceMembers)
                .Where(x => (x.FirstName.Contains(query) || x.LastName.Contains(query))
                    && x.PersonID != account.PersonID)
                .AsEnumerable();

            var members = _dbContext.Accounts
                .Include(x => x.Person)
                .Where(x => personMembers.Any(y => y.PersonID == x.PersonID))
                .Select(x => new Tuple<Person, Account>(x.Person, x))
                .AsEnumerable();

            var response = new PersonAccountResponse(members);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddDevice([FromBody] AccessDevicesRequest device)
        {
            if (_dbContext.AccessDevices.Any(x => x.MacAddress == device.MACAddress))
            {
                return NotFound(new BaseResponse("Device with given MAC address is already used"));
            }

            var newDevice = new AccessDevice
            {
                Name = device.Name,
                Description = device.Description,
                MacAddress = device.MACAddress
            };

            _dbContext.AccessDevices.Add(newDevice);
            _dbContext.SaveChanges();

            return Ok(new BaseResponse());
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult UpdateDevice(int id, [FromBody] AccessDevicesRequest device)
        {
            var deviceToUpdate = _dbContext.AccessDevices.FirstOrDefault(x => x.AccessDeviceID == id);
            if (deviceToUpdate == null)
            {
                return NotFound(new BaseResponse("Device not found"));
            }

            deviceToUpdate.Name = device.Name;
            deviceToUpdate.Description = device.Description;
            deviceToUpdate.MacAddress = device.MACAddress;

            _dbContext.AccessDevices.Update(deviceToUpdate);
            _dbContext.SaveChanges();
            return Ok(new BaseResponse());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteDevice(int id)
        {
            var device = _dbContext.AccessDevices.FirstOrDefault(x => x.AccessDeviceID == id);
            if (device == null)
            {
                return NotFound(new BaseResponse("Device not found"));
            }

            _dbContext.AccessDevices.Remove(device);
            _dbContext.SaveChanges();
            return Ok(new BaseResponse());
        }

        [HttpDelete("{id}/account/{account_id}")]
        public IActionResult RemoveMember(int id, int account_id)
        {
            var device = _dbContext.AccessDevices.FirstOrDefault(x => x.AccessDeviceID == id);
            if (device == null)
            {
                return NotFound(new BaseResponse("Device not found"));
            }

            var issuerAccount = _dbContext.Accounts.FirstOrDefault(x => x.Login == User.Identity.Name);
            if (issuerAccount == null)
            {
                return NotFound(new BaseResponse("Issuer account not found"));
            }

            var account = _dbContext.Accounts.FirstOrDefault(x => x.AccountID == account_id);
            if (account == null)
            {
                return NotFound(new BaseResponse("Account not found"));
            }

            var issuerAccess = _dbContext.Accesses.FirstOrDefault(x => x.AccessDeviceID == id && x.ReceiverAccountID == issuerAccount.AccountID);
            if (issuerAccess == null)
            {
                return NotFound(new BaseResponse("You don't have access to this device"));
            }

            if (!issuerAccess.AllowProvide)
            {
                return BadRequest(new BaseResponse("You can't remove members from device"));
            }

            var access = _dbContext.Accesses.FirstOrDefault(x => x.AccessDeviceID == id && x.ReceiverAccountID == account_id);
            if (access == null)
            {
                return NotFound(new BaseResponse("Member not found"));
            }
            
            if (access.ProviderAccountID == account_id)
            {
                return BadRequest(new BaseResponse("You can't remove provider from device"));
            }

            if (access.ReceiverAccountID == issuerAccount.AccountID)
            {
                return BadRequest(new BaseResponse("You can't remove yourself from device"));
            }

            _dbContext.Accesses.Remove(access);
            _dbContext.SaveChanges();
            return Ok(new BaseResponse());
        }
    }
}
