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
    public class DeleteModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public DeleteModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DebtorGroup = await _context.DebtorGroup.FindAsync(id);

            if (DebtorGroup != null)
            {
                _context.DebtorGroup.Remove(DebtorGroup);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
