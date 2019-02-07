using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;

namespace BillingNextSys.Pages.Bill
{
    public class DeleteModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public DeleteModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Bill Bill { get; set; }

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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Bill = await _context.Bill.FindAsync(id);

            if (Bill != null)
            {
                _context.Bill.Remove(Bill);
                await _context.SaveChangesAsync();

                var DebtorGroupOut = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(Bill.DebtorGroupID)).Select(a => a.DebtorOutstanding).FirstOrDefault();

                var billout = DebtorGroupOut - Bill.BillAmount;
                var dgout = new Models.DebtorGroup { DebtorGroupID = Bill.DebtorGroupID, DebtorOutstanding = billout };
                _context.DebtorGroup.Attach(dgout).Property(x => x.DebtorOutstanding).IsModified = true;
                _context.SaveChanges();
            }

            return RedirectToPage("./Index");
        }
    }
}
