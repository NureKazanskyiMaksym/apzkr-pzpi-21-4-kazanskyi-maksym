using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EquipmentWatcher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication", Roles = "Secretary")]
    public class ReportsController : Controller
    {
        private readonly DbApp _dbContext;

        public ReportsController(DbApp dbApp)
        {
            _dbContext = dbApp;
        }

        [HttpGet("persons")]
        public IActionResult GetPersons()
        {
            var persons = _dbContext.Persons.AsEnumerable();

            var stream = new MemoryStream();
            var pdfWriter = new PdfWriter(stream);
            var pdfDocument = new PdfDocument(pdfWriter.SetSmartMode(true));

            pdfDocument.AddNewPage();

            var headerFont = PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD);

            var header = new Paragraph("Persons")
                .SetFont(headerFont)
                .SetFontSize(16)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

            using (var document = new Document(pdfDocument))
            {
                document.Add(header);

                var table = new Table(new float[] { 1, 1, 1, 1, 2 });

                table.AddCell(new Cell().Add(new Paragraph("ID").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("FirstName").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("MiddleName").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("LastName").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("Email").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));

                foreach (var person in persons)
                {
                    table.AddCell(new Cell().Add(new Paragraph(person.PersonID.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(person.FirstName)));
                    table.AddCell(new Cell().Add(new Paragraph(person.MiddleName)));
                    table.AddCell(new Cell().Add(new Paragraph(person.LastName)));
                    table.AddCell(new Cell().Add(new Paragraph(person.Email)));
                }

                document.Add(table);
            }
            pdfDocument.Close();

            var pdfBytes = stream.ToArray();

            return File(pdfBytes, "application/pdf", "Persons.pdf");
        }

        [HttpGet("accounts")]
        public IActionResult GetAccounts()
        {
            var accounts = _dbContext.Accounts.AsEnumerable();

            var stream = new MemoryStream();
            var pdfWriter = new PdfWriter(stream);
            var pdfDocument = new PdfDocument(pdfWriter.SetSmartMode(true));

            pdfDocument.AddNewPage();

            var headerFont = PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD);

            var header = new Paragraph("Accounts")
                .SetFont(headerFont)
                .SetFontSize(16)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

            using (var document = new Document(pdfDocument))
            {
                document.Add(header);

                var table = new Table(new float[] { 1, 1, 1, 2 });

                table.AddCell(new Cell().Add(new Paragraph("ID").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("PersonID").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("Login").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("LastSession").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));

                foreach (var account in accounts)
                {
                    table.AddCell(new Cell().Add(new Paragraph(account.AccountID.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(account.PersonID.ToString())));
                    table.AddCell(new Cell().Add(new Paragraph(account.Login)));
                    table.AddCell(new Cell().Add(new Paragraph(account.LastSession.ToString())));
                }

                document.Add(table);
            }
            pdfDocument.Close();

            var pdfBytes = stream.ToArray();

            return File(pdfBytes, "application/pdf", "Accounts.pdf");
        }

        [HttpGet("access_devices")]
        public IActionResult GetAccessDevices()
        {
            var accessDevices = _dbContext.AccessDevices.AsEnumerable();

            var stream = new MemoryStream();
            var pdfWriter = new PdfWriter(stream);
            var pdfDocument = new PdfDocument(pdfWriter.SetSmartMode(true));

            pdfDocument.AddNewPage();

            var headerFont = PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD);

            var header = new Paragraph("Access Devices")
                .SetFont(headerFont)
                .SetFontSize(16)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

            using (var document = new Document(pdfDocument))
            {
                document.Add(header);

                var table = new Table(new float[] { 1, 1, 1, 1 });

                table.AddCell(new Cell().Add(new Paragraph("ID").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("Name").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("Description").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("MACAddress").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));

                foreach (var accessDevice in accessDevices)
                {
                    table.AddCell(new Cell().Add(new Paragraph(accessDevice.AccessDeviceID.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(accessDevice.Name)));
                    table.AddCell(new Cell().Add(new Paragraph(accessDevice.Description)));
                    table.AddCell(new Cell().Add(new Paragraph(accessDevice.MacAddress)));
                }

                document.Add(table);
            }
            pdfDocument.Close();

            var pdfBytes = stream.ToArray();

            return File(pdfBytes, "application/pdf", "AccessDevices.pdf");
        }

        [HttpGet("access_tokens")]
        public IActionResult GetAccessTokens()
        {
            var accessTokens = _dbContext.AccessTokens.AsEnumerable();

            var stream = new MemoryStream();
            var pdfWriter = new PdfWriter(stream);
            var pdfDocument = new PdfDocument(pdfWriter.SetSmartMode(true));

            pdfDocument.AddNewPage();

            var headerFont = PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD);

            var header = new Paragraph("Access Tokens")
                .SetFont(headerFont)
                .SetFontSize(16)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

            using (var document = new Document(pdfDocument))
            {
                document.Add(header);

                var table = new Table(new float[] { 1, 1, 1, 1 });

                table.AddCell(new Cell().Add(new Paragraph("ID").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("AccountID").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("Token").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("ExpiresOn").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));

                foreach (var accessToken in accessTokens)
                {
                    table.AddCell(new Cell().Add(new Paragraph(accessToken.AccessTokenID.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(accessToken.AccountID.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(accessToken.Token)));
                    table.AddCell(new Cell().Add(new Paragraph(accessToken.ExpiresOn.ToString())));
                }

                document.Add(table);
            }
            pdfDocument.Close();

            var pdfBytes = stream.ToArray();

            return File(pdfBytes, "application/pdf", "AccessTokens.pdf");
        }

        [HttpGet("accesses")]
        public IActionResult GetAccesses()
        {
            var accesses = _dbContext.Accesses.Include(x => x.AccessDevice)
                .Include(x => x.Interactions)
                .Include(x => x.ProviderAccount)
                .Include(x => x.ReceiverAccount)
                .AsEnumerable();

            var stream = new MemoryStream();
            var pdfWriter = new PdfWriter(stream);
            var pdfDocument = new PdfDocument(pdfWriter.SetSmartMode(true));

            pdfDocument.AddNewPage();

            var headerFont = PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD);

            var header = new Paragraph("Accesses")
                .SetFont(headerFont)
                .SetFontSize(16)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

            using (var document = new Document(pdfDocument))
            {
                document.Add(header);

                var table = new Table(new float[] { 1, 1, 1, 1, 1, 1 });

                table.AddCell(new Cell().Add(new Paragraph("ID").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("Provider\nID").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("Receiver\nID").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("Device\nID").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("Created").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("Expires").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("AllowProvide").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));

                foreach (var accessDevice in accesses)
                {
                    table.AddCell(new Cell().Add(new Paragraph(accessDevice.AccessID.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(accessDevice.ProviderAccountID.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(accessDevice.ReceiverAccountID.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(accessDevice.AccessDeviceID.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(accessDevice.CreatedAt.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(accessDevice.ExpiresOn.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(accessDevice.AllowProvide.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                }

                document.Add(table);
            }
            pdfDocument.Close();

            var pdfBytes = stream.ToArray();

            return File(pdfBytes, "application/pdf", "Accesses.pdf");
        }

        [HttpGet("interactions")]
        public IActionResult GetInteractions()
        {
            var interactions = _dbContext.Interactions.Include(x => x.Access)
                .AsEnumerable();

            var stream = new MemoryStream();
            var pdfWriter = new PdfWriter(stream);
            var pdfDocument = new PdfDocument(pdfWriter.SetSmartMode(true));

            pdfDocument.AddNewPage();

            var headerFont = PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD);

            var header = new Paragraph("Interactions")
                .SetFont(headerFont)
                .SetFontSize(16)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

            using (var document = new Document(pdfDocument))
            {
                document.Add(header);

                var table = new Table(new float[] { 1, 1, 1, 1, 1 });

                table.AddCell(new Cell().Add(new Paragraph("ID").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("Access\nID").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("Token").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("Timestamp").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("Result").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));

                foreach (var interaction in interactions)
                {
                    table.AddCell(new Cell().Add(new Paragraph(interaction.InteractionID.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(interaction.AccessID.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(interaction.Token).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(interaction.Timestamp.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(interaction.Result)));
                }

                document.Add(table);
            }
            pdfDocument.Close();

            var pdfBytes = stream.ToArray();

            return File(pdfBytes, "application/pdf", "Interactions.pdf");
        }

        [HttpGet("permissions")]
        public IActionResult GetPermissions()
        {
            var permissions = _dbContext.Permissions.AsEnumerable();

            var stream = new MemoryStream();
            var pdfWriter = new PdfWriter(stream);
            var pdfDocument = new PdfDocument(pdfWriter.SetSmartMode(true));

            pdfDocument.AddNewPage();

            var headerFont = PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD);

            var header = new Paragraph("Permissions")
                .SetFont(headerFont)
                .SetFontSize(16)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

            using (var document = new Document(pdfDocument))
            {
                document.Add(header);

                var table = new Table(new float[] { 1, 1, 1, 1 });

                table.AddCell(new Cell().Add(new Paragraph("ID").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("AccountID").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                table.AddCell(new Cell().Add(new Paragraph("Value").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));

                foreach (var permission in permissions)
                {
                    table.AddCell(new Cell().Add(new Paragraph(permission.PermissionID.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(permission.AccountID.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(permission.Value)));
                }

                document.Add(table);
            }
            pdfDocument.Close();

            var pdfBytes = stream.ToArray();

            return File(pdfBytes, "application/pdf", "Permissions.pdf");
        }
    }
}

