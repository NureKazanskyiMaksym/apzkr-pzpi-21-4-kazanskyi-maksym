using EquipmentWatcher.Models;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace EquipmentWatcher.Pages.Reports.Accounts
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly EquipmentWatcher.DbApp _context;

        public IndexModel(EquipmentWatcher.DbApp context)
        {
            _context = context;
        }

        public EquipmentWatcher.Models.Account Account { get; set; } = default!;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public bool IsAccountID { get; set; }
            public string? AccountIDFilter { get; set; } = default!;
            public bool IsPersonID { get; set; }
            public string? PersonIDFilter { get; set; } = default!;
            public bool IsLogin { get; set; }
            public string? LoginFilter { get; set; } = default!;
            public bool IsPassword { get; set; }
            public string? PasswordFilter { get; set; } = default!;
            public bool IsLastSession { get; set; }
            public DateTime? LastSessionAfterFilter { get; set; } = default!;
            public DateTime? LastSessionBeforeFilter { get; set; } = default!;
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

            /*var columnCount = Convert.ToInt32(Input.IsAccountID) + Convert.ToInt32(Input.IsPersonID) + Convert.ToInt32(Input.IsLogin) + Convert.ToInt32(Input.IsPassword) + Convert.ToInt32(Input.IsLastSession);

            if (columnCount == 0)
            {
                ModelState.AddModelError(string.Empty, "Select at least one column.");
                return Page();
            }

            var user = _context.Accounts.Include(x => x.Person)
                .First(x => x.Login == User.Identity.Name);

            int[]? accountIDs = null;
            int[]? personIDs = null;
            string[]? logins = null;
            string[]? passwords = null;

            if (Input.AccountIDFilter != null)
            {
                accountIDs = Input.AccountIDFilter.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            }
            if (Input.PersonIDFilter != null)
            {
                personIDs = Input.PersonIDFilter.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            }
            if (Input.LoginFilter != null)
            {
                logins = Input.LoginFilter.Split(',', StringSplitOptions.RemoveEmptyEntries);
            }
            if (Input.PasswordFilter != null)
            {
                passwords = Input.PasswordFilter.Split(',', StringSplitOptions.RemoveEmptyEntries);
            }

            var accounts = _context.Accounts.Where(
                delegate(EquipmentWatcher.Models.Account x)
                {
                    return
                        (accountIDs == null || accountIDs.Contains(x.AccountID)) &&
                        (personIDs == null || personIDs.Contains(x.PersonID)) &&
                        (logins == null || logins.Contains(x.Login)) &&
                        (passwords == null || passwords.Contains(x.Password)) &&
                        (Input.LastSessionAfterFilter == null || x.LastSession >= Input.LastSessionAfterFilter) &&
                        (Input.LastSessionBeforeFilter == null || x.LastSession <= Input.LastSessionBeforeFilter);
                })
                .AsEnumerable();

            if (!accounts.Any())
            {
                ModelState.AddModelError(string.Empty, "No records found.");
                return Page(); 
            }

            var columns = new Dictionary<string, bool>
            {
                { "AccountID", Input.IsAccountID },
                { "PersonID", Input.IsPersonID },
                { "Login", Input.IsLogin },
                { "Password", Input.IsPassword },
                { "LastSession", Input.IsLastSession }
            };

            var data = accounts.Select(a => new Dictionary<string, string>{
                { "AccountID", a.AccountID.ToString() },
                { "PersonID", a.PersonID.ToString() },
                { "Login", a.Login },
                { "Password", a.Password },
                { "LastSession", a.LastSession.ToString("u") },
            });

            var filters = new Dictionary<string, string>
            {
                { "AccountID", Input.AccountIDFilter ?? "" },
                { "PersonID", Input.PersonIDFilter ?? "" },
                { "Login", Input.LoginFilter ?? "" },
                { "Password", Input.PasswordFilter ?? "" },
                { "LastSessionAfter", Input.LastSessionAfterFilter?.ToString("u") ?? "" },
                { "LastSessionBefore", Input.LastSessionBeforeFilter?.ToString("u") ?? "" },
            };

            PdfReportUtility.GenerateReport("Accounts Report", columns, data, $"{user.Person.FirstName} {user.Person.MiddleName}{(user.Person.MiddleName == "" ? "" : " ")}{user.Person.LastName}", filters);

            var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdfDocument = new PdfDocument(writer.SetSmartMode(true));

            pdfDocument.AddNewPage();

            var headerFont = PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD);

            var header = new Paragraph("Accounts")
                .SetFont(headerFont)
                .SetFontSize(24)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

            var sb = new StringBuilder();

            sb.AppendLine("Generated at: " + DateTime.UtcNow.ToString("u"));
            sb.AppendLine("Issuer: " + $"{user.Person.FirstName} {user.Person.MiddleName}{(user.Person.MiddleName == "" ? "" : " ")}{user.Person.LastName}");
            sb.Append("Raw filters:");

            if (Input.AccountIDFilter == null && Input.PersonIDFilter == null && Input.LoginFilter == null && Input.PasswordFilter == null && Input.LastSessionBeforeFilter == null && Input.LastSessionAfterFilter == null)
            {
                sb.AppendLine(" none.");
            }
            else
            {
                sb.AppendLine();
                if (Input.AccountIDFilter != null)
                {
                    sb.AppendLine($"AccountID: {Input.AccountIDFilter}");
                }
                if (Input.PersonIDFilter != null)
                {
                    sb.AppendLine($"PersonID: {Input.PersonIDFilter}");
                }
                if (Input.LoginFilter != null)
                {
                    sb.AppendLine($"Login: {Input.LoginFilter}");
                }
                if (Input.PasswordFilter != null)
                {
                    sb.AppendLine($"Password: {Input.PasswordFilter}");
                }
                if (Input.LastSessionAfterFilter != null)
                {
                    sb.AppendLine($"LastSessionAfter: {Input.LastSessionAfterFilter}");
                }
                if (Input.LastSessionBeforeFilter != null)
                {
                    sb.AppendLine($"LastSessionBefore: {Input.LastSessionBeforeFilter}");
                }
            }

            var info = new Paragraph(sb.ToString())
                .SetFont(headerFont)
                .SetFontSize(12)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT);

            using (var document = new Document(pdfDocument))
            {
                document.Add(header);
                document.Add(info);

                var table = new Table(new float[columnCount]);

                if (Input.IsAccountID)
                {
                    table.AddCell(new Cell().Add(new Paragraph("AccountID").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                }
                if (Input.IsPersonID)
                {
                    table.AddCell(new Cell().Add(new Paragraph("PersonID").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                }
                if (Input.IsLogin)
                {
                    table.AddCell(new Cell().Add(new Paragraph("Login").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                }
                if (Input.IsPassword)
                {
                    table.AddCell(new Cell().Add(new Paragraph("Password").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                }
                if (Input.IsLastSession)
                {
                    table.AddCell(new Cell().Add(new Paragraph("LastSession").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                }

                foreach (var account in accounts)
                {
                    if (Input.IsAccountID)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(account.AccountID.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    }
                    if (Input.IsPersonID)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(account.PersonID.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    }
                    if (Input.IsLogin)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(account.Login)));
                    }
                    if (Input.IsPassword)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(account.Password)));
                    }
                    if (Input.IsLastSession)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(account.LastSession.ToString("u"))));
                    }
                }

                document.Add(table);
            }
            pdfDocument.Close();*/

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
