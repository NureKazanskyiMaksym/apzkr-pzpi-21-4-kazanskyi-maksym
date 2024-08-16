using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EquipmentWatcher;
using EquipmentWatcher.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EquipmentWatcher.Pages.Models.AccessTokens
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly EquipmentWatcher.DbApp _context;

        public EditModel(EquipmentWatcher.DbApp context)
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

            var accesstoken =  await _context.AccessTokens.FirstOrDefaultAsync(m => m.AccessTokenID == id);
            if (accesstoken == null)
            {
                return NotFound();
            }
            AccessToken = accesstoken;
           ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "Login");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(AccessToken).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccessTokenExists(AccessToken.AccessTokenID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AccessTokenExists(int id)
        {
            return _context.AccessTokens.Any(e => e.AccessTokenID == id);
        }
    }
}
