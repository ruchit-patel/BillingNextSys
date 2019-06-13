using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;
using Microsoft.AspNetCore.Authorization;

namespace BillingNextSys.PagesReceived
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public DeleteModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Received Received { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Received = await _context.Received
                .Include(r => r.BillDetails)
                .Include(r=>r.DebtorGroup)
                .Include(r => r.Company).FirstOrDefaultAsync(m => m.ReceivedID == id);

            if (Received == null)
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

            Received = await _context.Received.FindAsync(id);

            if (Received != null)
            {
                _context.Received.Remove(Received);
                await _context.SaveChangesAsync();

                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                var amtout = _context.BillDetails.Where(a => a.BillDetailsID.Equals(Received.BillDetailsID)).FirstOrDefault().BillAmountOutstanding;
                // var amtout = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(dgid)).FirstOrDefault().DebtorOutstanding;
                var billout = amtout + Received.ReceivedAmount;
                var billdet = new Models.BillDetails { BillDetailsID = Received.BillDetailsID, BillAmountOutstanding = billout };
                _context.BillDetails.Attach(billdet).Property(x => x.BillAmountOutstanding).IsModified = true;
                _context.SaveChanges();

                var dgout = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(Received.DebtorGroupID)).FirstOrDefault().DebtorOutstanding;

                var debout = dgout + Received.ReceivedAmount;
                var dgdet = new Models.DebtorGroup { DebtorGroupID = Received.DebtorGroupID, DebtorOutstanding = debout };
                _context.DebtorGroup.Attach(dgdet).Property(x => x.DebtorOutstanding).IsModified = true;
                _context.SaveChanges();
            }
                return RedirectToPage("./Index", new { companyid = Received.CompanyID});
        }
    }
}
