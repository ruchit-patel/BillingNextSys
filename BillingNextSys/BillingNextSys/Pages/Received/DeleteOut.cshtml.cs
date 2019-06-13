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
    public class DeleteOutModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public DeleteOutModel(BillingNextSys.Models.BillingNextSysContext context)
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
                .Include(r=>r.DebtorGroup)
                .FirstOrDefaultAsync(m => m.ReceivedID == id);

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

                var dgout = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(Received.DebtorGroupID)).FirstOrDefault().DebtorOutstanding;

                var debout = dgout + Received.ReceivedAmount;
                var dgdet = new Models.DebtorGroup { DebtorGroupID = Received.DebtorGroupID, DebtorOutstanding = debout };
                _context.DebtorGroup.Attach(dgdet).Property(x => x.DebtorOutstanding).IsModified = true;
                _context.SaveChanges();
            }

            return RedirectToPage("./Outstanding");
        }
    }
}
