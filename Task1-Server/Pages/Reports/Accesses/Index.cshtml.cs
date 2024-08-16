using EquipmentWatcher.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EquipmentWatcher.Pages.Reports.Accesses
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public readonly EquipmentWatcher.DbApp _context;

        public IndexModel(EquipmentWatcher.DbApp context)
        {
            _context = context;
        }

        public Access Access { get; set; } = default!;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public bool IsAccessID { get; set; }
            public string? AccessIDFilter { get; set; } = default!;
            public bool IsProviderAccountID { get; set; }
            public string? ProviderAccountIDFilter { get; set; } = default!;
            public bool IsReceiverAccountID { get; set; }
            public string? ReceiverAccountIDFilter { get; set; } = default!;
            public bool IsAccessDeviceID { get; set; }
            public string? AccessDeviceIDFilter { get; set; } = default!;
            public bool IsCreatedAt { get; set; }
            public DateTime? CreatedAtAfterFilter { get; set; } = default!;
            public DateTime? CreatedAtBeforeFilter { get; set; } = default!;
            public bool IsExpiresOn { get; set; }
            public DateTime? ExpiresOnAfterFilter { get; set; } = default!;
            public DateTime? ExpiresOnBeforeFilter { get; set; } = default!;
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

            try
            {
                var stream = this.GenerateReport(_context, Input);

                return File(stream.ToArray(), "application/pdf", $"AccessDevices-{DateTime.UtcNow}.pdf");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
