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

namespace EquipmentWatcher.Pages.Models.AccessDevice
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
        public EquipmentWatcher.Models.AccessDevice AccessDevice { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessdevice =  await _context.AccessDevices.FirstOrDefaultAsync(m => m.AccessDeviceID == id);
            if (accessdevice == null)
            {
                return NotFound();
            }
            AccessDevice = accessdevice;
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

            _context.Attach(AccessDevice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccessDeviceExists(AccessDevice.AccessDeviceID))
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

        private bool AccessDeviceExists(int id)
        {
            return _context.AccessDevices.Any(e => e.AccessDeviceID == id);
        }
    }
}
