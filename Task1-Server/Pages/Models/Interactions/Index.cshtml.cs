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

namespace EquipmentWatcher.Pages.Models.Interactions
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly EquipmentWatcher.DbApp _context;

        public IndexModel(EquipmentWatcher.DbApp context)
        {
            _context = context;
        }

        public IList<Interaction> Interaction { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Interaction = await _context.Interactions
                .Include(i => i.Access).ToListAsync();
        }
    }
}
