using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BillingNextSys.Pages.Bill
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public DeleteModel(BillingNextSys.Models.BillingNextSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
             _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public Models.Bill Bill { get; set; }

        public double amtoutstnding { get; set; }

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

            amtoutstnding= _context.BillDetails.Where(a => a.BillNumber.Equals(Bill.BillNumber)).Select(a => a.BillAmountOutstanding).Sum();
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
                var billdetsum = _context.BillDetails.Where(a => a.BillNumber.Equals(Bill.BillNumber)).Select(a => a.BillAmountOutstanding).Sum();
                _context.Bill.Remove(Bill);
                await _context.SaveChangesAsync();

                var DebtorGroupOut = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(Bill.DebtorGroupID)).Select(a => a.DebtorOutstanding).FirstOrDefault();

                var billout = DebtorGroupOut - billdetsum;
                var dgout = new Models.DebtorGroup { DebtorGroupID = Bill.DebtorGroupID, DebtorOutstanding = billout };
                _context.DebtorGroup.Attach(dgout).Property(x => x.DebtorOutstanding).IsModified = true;
                _context.SaveChanges();
            }
            if (Bill.SeriesName != null)
            {
                return RedirectToPage("/Bill/Format1/Index");
            }
            else
            {
                return RedirectToPage("/Bill/Format2/Index");
            }
        }
        public IActionResult OnGetBackToList()
        {
            try
            {
                int cid = (int)_session.GetInt32("Cid");
                int bid = (int)_session.GetInt32("Bid");
                return Page();
            }
            catch(InvalidOperationException)
            {
               return RedirectToPage("/Index");
            }
        }
    }
}
