using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;

namespace BillingNextSys.Pages.AdvancePay
{
    public class IndexModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public IndexModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public IList<Models.AdvancePay> AdvancePay { get;set; }

        public async Task OnGetAsync()
        {
            AdvancePay = await _context.AdvancePay
                .Include(a => a.Branch)
                .Include(a => a.Company)
                .Include(a => a.DebtorGroup).ToListAsync();
        }
    }
}
