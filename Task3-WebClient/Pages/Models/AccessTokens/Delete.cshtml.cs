using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EquipmentWatcher;
using EquipmentWatcher.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EquipmentWatcher.Pages.Models.AccessTokens
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly EquipmentWatcher.DbApp _context;

        public DeleteModel(EquipmentWatcher.DbApp context)
        {
            _context = context;
        }

        [BindProperty]
        public AccessToken AccessToken { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accesstoken = await _context.AccessTokens.FirstOrDefaultAsync(m => m.AccessTokenID == id);

            if (accesstoken == null)
            {
                return NotFound();
            }
            else
            {
                AccessToken = accesstoken;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accesstoken = await _context.AccessTokens.FindAsync(id);
            if (accesstoken != null)
            {
                AccessToken = accesstoken;
                _context.AccessTokens.Remove(AccessToken);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
