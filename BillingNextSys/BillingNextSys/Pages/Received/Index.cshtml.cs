using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;

namespace BillingNextSys.Pages.Received
{
    public class IndexModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public IndexModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public IList<Models.Received> Received { get;set; }

        public async Task OnGetAsync()
        {
            Received = await _context.Received
                .Include(r => r.BillDetails)
                .Include(r => r.Company).ToListAsync();
        }
    }
}
