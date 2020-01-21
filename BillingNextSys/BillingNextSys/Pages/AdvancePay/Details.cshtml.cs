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
    public class DetailsModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public DetailsModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public Models.AdvancePay AdvancePay { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AdvancePay = await _context.AdvancePay
                .Include(a => a.Branch)
                .Include(a => a.Company)
                .Include(a => a.DebtorGroup).FirstOrDefaultAsync(m => m.AdvancePayID == id);

            if (AdvancePay == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
