using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;

namespace BillingNextSys.PagesBill
{
    public class DetailsModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public DetailsModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public Bill Bill { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Bill = await _context.Bill
                .Include(b => b.BillSeries)
                .Include(b => b.Company)
                .Include(b => b.DebtorGroup).FirstOrDefaultAsync(m => m.BillNumber == id);

            if (Bill == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
