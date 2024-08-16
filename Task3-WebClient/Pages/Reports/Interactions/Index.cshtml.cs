using EquipmentWatcher.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EquipmentWatcher.Pages.Reports.Interactions
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public readonly EquipmentWatcher.DbApp _context;

        public IndexModel(EquipmentWatcher.DbApp context)
        {
            _context = context;
        }

        public Interaction Interaction { get; set; } = default!;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public bool IsInteractionID { get; set; }
            public string? InteractionIDFilter { get; set; } = default!;
            public bool IsAccessID { get; set; }
            public string? AccessIDFilter { get; set; } = default!;
            public bool IsToken { get; set; }
            public string? TokenFilter { get; set; } = default!;
            public bool IsTimestamp { get; set; }
            public DateTime? TimestampAfterFilter { get; set; } = default!;
            public DateTime? TimestampBeforeFilter { get; set; } = default!;
            public bool IsResult { get; set; }
            public string? ResultFilter { get; set; } = default!;
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

                return File(stream.ToArray(), "application/pdf", $"Interactions-{DateTime.UtcNow}.pdf");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
