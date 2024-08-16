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

namespace EquipmentWatcher.Pages.Models.Accesses
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
        public Access Access { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var access =  await _context.Accesses.FirstOrDefaultAsync(m => m.AccessID == id);
            if (access == null)
            {
                return NotFound();
            }
            Access = access;
           ViewData["AccessDeviceID"] = new SelectList(_context.AccessDevices, "AccessDeviceID", "Name");
           ViewData["ProviderAccountID"] = new SelectList(_context.Accounts, "AccountID", "Login");
           ViewData["ReceiverAccountID"] = new SelectList(_context.Accounts, "AccountID", "Login");
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

            _context.Attach(Access).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccessExists(Access.AccessID))
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

        private bool AccessExists(int id)
        {
            return _context.Accesses.Any(e => e.AccessID == id);
        }
    }
}
