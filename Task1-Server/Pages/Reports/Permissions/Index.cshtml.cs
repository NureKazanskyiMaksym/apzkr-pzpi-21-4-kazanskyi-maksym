using EquipmentWatcher.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EquipmentWatcher.Pages.Reports.Permissions
{
    public class IndexModel : PageModel
    {
        public readonly EquipmentWatcher.DbApp _context;

        public IndexModel(EquipmentWatcher.DbApp context)
        {
            _context = context;
        }

        public Permission Permission { get; set; } = default!;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public bool IsPermissionID { get; set; }
            public string? PermissionIDFilter { get; set; } = default!;
            public bool IsAccountID { get; set; }
            public string? AccountIDFilter { get; set; } = default!;
            public bool IsValue { get; set; }
            public string? ValueFilter { get; set; } = default!;
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

                return File(stream.ToArray(), "application/pdf", $"Permissions-{DateTime.UtcNow}.pdf");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
