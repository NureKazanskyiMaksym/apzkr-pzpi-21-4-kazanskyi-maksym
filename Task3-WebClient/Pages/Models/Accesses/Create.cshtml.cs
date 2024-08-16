using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using EquipmentWatcher;
using EquipmentWatcher.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EquipmentWatcher.Pages.Models.Accesses
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly EquipmentWatcher.DbApp _context;

        public CreateModel(EquipmentWatcher.DbApp context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["AccessDeviceID"] = new SelectList(_context.AccessDevices, "AccessDeviceID", "Name");
        ViewData["ProviderAccountID"] = new SelectList(_context.Accounts, "AccountID", "Login");
        ViewData["ReceiverAccountID"] = new SelectList(_context.Accounts, "AccountID", "Login");
            return Page();
        }

        [BindProperty]
        public Access Access { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Accesses.Add(Access);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
