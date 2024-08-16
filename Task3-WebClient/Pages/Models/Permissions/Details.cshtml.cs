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

namespace EquipmentWatcher.Pages.Models.Permissions
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly EquipmentWatcher.DbApp _context;

        public DetailsModel(EquipmentWatcher.DbApp context)
        {
            _context = context;
        }

        public Permission Permission { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permission = await _context.Permissions.FirstOrDefaultAsync(m => m.PermissionID == id);
            if (permission == null)
            {
                return NotFound();
            }
            else
            {
                Permission = permission;
            }
            return Page();
        }
    }
}
