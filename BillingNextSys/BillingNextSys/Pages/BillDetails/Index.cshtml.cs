using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;

namespace BillingNextSys.Pages.BillDetails
{
    public class IndexModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public IndexModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public IList<Models.BillDetails> BillDetails { get;set; }

        public async Task OnGetAsync()
        {
            BillDetails = await _context.BillDetails
                .Include(b => b.Bill)
                .Include(b => b.Company)
                .Include(b => b.Debtor)
                .Include(b => b.DebtorGroup)
                .Include(b => b.Particulars).ToListAsync();
        }
    }
}
