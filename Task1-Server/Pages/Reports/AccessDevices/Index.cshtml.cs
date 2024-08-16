using EquipmentWatcher.Models;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EquipmentWatcher.Pages.Reports.AccessDevices
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DbApp _context;

        public IndexModel(DbApp context)
        {
            _context = context;
        }

        public EquipmentWatcher.Models.AccessDevice AccessDevice { get; set; } = default!;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public bool IsAccessDeviceID { get; set; }
            public string? AccessDeviceIDFilter { get; set; } = default!;
            public bool IsName { get; set; }
            public string? NameFilter { get; set; } = default!;
            public bool IsDescription { get; set; }
            public string? DescriptionFilter { get; set; } = default!;
            public bool IsMACAddress { get; set; }
            public string? MACAddressFilter { get; set; } = default!;
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

            /*var columnCount = Convert.ToInt32(Input.IsAccessDeviceID) + Convert.ToInt32(Input.IsName) + Convert.ToInt32(Input.IsDescription) + Convert.ToInt32(Input.IsMACAddress);

            if (columnCount == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select at least one column.");
                return Page();
            }

            var user = _context.Accounts.Include(x => x.Person)
                .First(x => x.Login == User.Identity.Name);

            int[]? accessDeviceIDs = null;
            string[]? names = null;
            string[]? descriptions = null;
            string[]? macAddresses = null;

            if (Input.AccessDeviceIDFilter != null)
            {
                accessDeviceIDs = Input.AccessDeviceIDFilter.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            }
            if (Input.NameFilter != null)
            {
                names = Input.NameFilter.Split(',', StringSplitOptions.RemoveEmptyEntries);
            }
            if (Input.DescriptionFilter != null)
            {
                descriptions = Input.DescriptionFilter.Split(',', StringSplitOptions.RemoveEmptyEntries);
            }
            if (Input.MACAddressFilter != null)
            {
                macAddresses = Input.MACAddressFilter.Split(',', StringSplitOptions.RemoveEmptyEntries);
            }

            var accessDevices = _context.AccessDevices
                .Where(x => (accessDeviceIDs == null || accessDeviceIDs.Contains(x.AccessDeviceID))
                && (names == null || names.Contains(x.Name))
                && (descriptions == null || descriptions.Contains(x.Description))
                && (macAddresses == null || macAddresses.Contains(x.MACAddress)))
                .AsEnumerable();

            if (!accessDevices.Any())
            {
                ModelState.AddModelError(string.Empty, "No records found.");
                return Page();
            }

            var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdfDocument = new PdfDocument(writer.SetSmartMode(true));

            pdfDocument.AddNewPage();

            var headerFont = PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD);

            var header = new Paragraph("Access Devices")
                .SetFont(headerFont)
                .SetFontSize(24)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

            return Page();*/

            try
            {
                var stream = this.GenerateReport(_context, Input);

                return File(stream.ToArray(), "application/pdf", $"AccessDevices-{DateTime.UtcNow}.pdf");
            }catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
