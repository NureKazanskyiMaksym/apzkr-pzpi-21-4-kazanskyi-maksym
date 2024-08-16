using EquipmentWatcher.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EquipmentWatcher.Pages.Reports.AccessTokens
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public readonly EquipmentWatcher.DbApp _context;

        public IndexModel(EquipmentWatcher.DbApp context)
        {
            _context = context;
        }

        public AccessToken AccessToken { get; set; } = default!;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public bool IsAccessTokenID { get; set; }
            public string? AccessTokenIDFilter { get; set; } = default!;
            public bool IsAccountID { get; set; }
            public string? AccountIDFilter { get; set; } = default!;
            public bool IsToken { get; set; }
            public string? TokenFilter { get; set; } = default!;
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

                return File(stream.ToArray(), "application/pdf", $"AccessTokens-{DateTime.UtcNow}.pdf");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
