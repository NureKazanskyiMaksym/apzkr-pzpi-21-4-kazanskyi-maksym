using EquipmentWatcher.Models;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace EquipmentWatcher.Pages.Reports.Persons
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly EquipmentWatcher.DbApp _context;

        public IndexModel(EquipmentWatcher.DbApp context)
        {
            _context = context;
        }

        public Person Person { get; set; } = default!;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public bool IsPersonID { get; set; }
            public string? PersonIDFilter { get; set; } = default!;
            public bool IsFirstName { get; set; }
            public string? FirstNameFilter { get; set; } = default!;
            public bool IsMiddleName { get; set; }
            public string? MiddleNameFilter { get; set; } = default!;
            public bool IsLastName { get; set; }
            public string? LastNameFilter { get; set; } = default!;
            public bool IsEmail { get; set; }
            public string? EmailFilter { get; set; } = default!;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            /*var columnCount = Convert.ToInt32(Input.IsPersonID) + Convert.ToInt32(Input.IsFirstName) + Convert.ToInt32(Input.IsMiddleName) + Convert.ToInt32(Input.IsLastName) + Convert.ToInt32(Input.IsEmail);

            if (columnCount == 0)
            {
                ModelState.AddModelError(string.Empty, "No columns selected.");
                return Page();
            }

            var user = _context.Accounts.Include(x => x.Person)
                .First(x => x.Login == User.Identity.Name);

            string[]? personIDs = null;
            string[]? firstNames = null;
            string[]? middleNames = null;
            string[]? lastNames = null;
            string[]? emails = null;

            if (Input.PersonIDFilter != null)
            {
                personIDs = Input.PersonIDFilter.Split(',');
            }
            if (Input.FirstNameFilter != null)
            {
                firstNames = Input.FirstNameFilter.Split(',');
            }
            if (Input.MiddleNameFilter != null)
            {
                middleNames = Input.MiddleNameFilter.Split(',');
            }
            if (Input.LastNameFilter != null)
            {
                lastNames = Input.LastNameFilter.Split(',');
            }
            if (Input.EmailFilter != null)
            {
                emails = Input.EmailFilter.Split(',');
            }

            var persons = _context.Persons.Where(
                delegate(Person p)
                {
                    return (personIDs == null || personIDs.Any(x => p.PersonID.ToString().Contains(x))) &&
                        (firstNames == null || firstNames.Any(x => p.FirstName.Contains(x))) &&
                        (middleNames == null || middleNames.Any(x => p.MiddleName.Contains(x))) &&
                        (lastNames == null || lastNames.Any(x => p.LastName.Contains(x))) &&
                        (emails == null || emails.Any(x => p.Email.Contains(x)));
                })
                .AsEnumerable();

            if (!persons.Any())
            {
                ModelState.AddModelError(string.Empty, "No records found.");
                return Page();
            }*/

            //var stream = PdfReportUtility.GenerateReport("Persons Report", $"{user.Person.FirstName} {user.Person.MiddleName}{(user.Person.MiddleName == "" ? "" : " ")}{user.Person.LastName}", Input, persons);
            //var stream = PdfReportUtility.GenerateReport("Persons Report", user, Input, persons);
            try
            {
                var stream = this.GenerateReport(_context, Input);

                return File(stream.ToArray(), "application/pdf", $"Persons-{DateTime.UtcNow}.pdf");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
