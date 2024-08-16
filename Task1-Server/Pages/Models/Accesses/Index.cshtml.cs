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

namespace EquipmentWatcher.Pages.Models.Accesses
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly EquipmentWatcher.DbApp _context;

        public IndexModel(EquipmentWatcher.DbApp context)
        {
            _context = context;
        }

        public IList<Access> Access { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Access = await _context.Accesses
                .Include(a => a.AccessDevice)
                .Include(a => a.ProviderAccount)
                .Include(a => a.ReceiverAccount).ToListAsync();
        }
    }
}
