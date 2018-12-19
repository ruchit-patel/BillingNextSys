using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;

namespace BillingNextSys.PagesDebtorGroup
{
    public class DetailsModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public DetailsModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public DebtorGroup DebtorGroup { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DebtorGroup = await _context.DebtorGroup
                .Include(d => d.Branch).FirstOrDefaultAsync(m => m.DebtorGroupID == id);

            if (DebtorGroup == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
