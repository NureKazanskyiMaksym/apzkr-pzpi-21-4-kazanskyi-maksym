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
    public class PersonController : ControllerBase
    {
        private readonly DbApp _dbContext;

        public PersonController(DbApp dbApp)
        {
            _dbContext = dbApp;
        }

        [HttpGet]
        [Authorize(Roles = "Secretary")]
        public IActionResult GetPersons()
        {
            var persons = _dbContext.Persons.ToList();
            var response = new PersonResponse(persons);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Secretary")]
        public IActionResult GetPerson(int id)
        {
            var person = _dbContext.Persons.FirstOrDefault(p => p.PersonID == id);
            if (person == null)
            {
                return NotFound(new BaseResponse("Person not found"));
            }

            var response = new PersonResponse(person);
            return Ok(response);
        }

        [HttpGet("me")]
        public IActionResult GetMe()
        {
            var account = _dbContext.Accounts.Include(x => x.Person)
                .FirstOrDefault(p => p.Login == User.Identity.Name);

            if (account == null)
            {
                return NotFound(new BaseResponse("Account not found"));
            }

            var person = account.Person;

            var response = new PersonResponse(person);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddPerson([FromBody] PersonRequest personRequest)
        {
            var newPerson = new Person
            {
                FirstName = personRequest.FirstName,
                MiddleName = personRequest.Midname,
                LastName = personRequest.LastName,
                Email = personRequest.Email
            };
            _dbContext.Persons.Add(newPerson);

            _dbContext.SaveChanges();

            return Ok(new BaseResponse());
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult UpdatePerson(int id, [FromBody] PersonRequest personRequest)
        {
            var person = _dbContext.Persons.FirstOrDefault(p => p.PersonID == id);
            if (person == null)
            {
                return NotFound(new BaseResponse("Person not found"));
            }

            person.FirstName = personRequest.FirstName;
            person.MiddleName = personRequest.Midname;
            person.LastName = personRequest.LastName;
            person.Email = personRequest.Email;

            _dbContext.SaveChanges();

            return Ok(new BaseResponse());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeletePerson(int id)
        {
            var person = _dbContext.Persons.FirstOrDefault(p => p.PersonID == id);
            if (person == null)
            {
                return NotFound(new BaseResponse("Person not found"));
            }

            _dbContext.Persons.Remove(person);
            _dbContext.SaveChanges();

            return Ok(new BaseResponse());
        }
    }
}
