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
    public class DeleteModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public DeleteModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.BillDetails BillDetails { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BillDetails = await _context.BillDetails
                .Include(b => b.Bill)
                .Include(b => b.Company)
                .Include(b => b.Debtor)
                .Include(b => b.DebtorGroup)
                .Include(b => b.Particulars).FirstOrDefaultAsync(m => m.BillDetailsID == id);

            if (BillDetails == null)
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

            BillDetails = await _context.BillDetails.FindAsync(id);

            if (BillDetails != null)
            {
                _context.BillDetails.Remove(BillDetails);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
