using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;

namespace BillingNextSys.PagesBill
{
    public class EditModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public EditModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           ViewData["SeriesName"] = new SelectList(_context.Set<BillSeries>(), "SeriesName", "SeriesName");
           ViewData["CompanyID"] = new SelectList(_context.Company, "CompanyID", "AccountNumber");
           ViewData["DebtorGroupID"] = new SelectList(_context.DebtorGroup, "DebtorGroupID", "DebtorGroupAddress");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Bill).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillExists(Bill.BillNumber))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BillExists(string id)
        {
            return _context.Bill.Any(e => e.BillNumber == id);
        }
    }
}
