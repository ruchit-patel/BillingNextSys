using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;

namespace BillingNextSys.Pages.BillDetails
{
    public class EditModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public EditModel(BillingNextSys.Models.BillingNextSysContext context)
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
           ViewData["BillNumber"] = new SelectList(_context.Bill, "BillNumber", "BillNumber");
           ViewData["CompanyID"] = new SelectList(_context.Company, "CompanyID", "CompanyName");
           ViewData["DebtorID"] = new SelectList(_context.Debtor, "DebtorID", "DebtorName");
           ViewData["DebtorGroupID"] = new SelectList(_context.DebtorGroup, "DebtorGroupID", "DebtorGroupName");
           ViewData["ParticularsID"] = new SelectList(_context.Set<Particulars>(), "ParticularsID", "ParticularsName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(BillDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillDetailsExists(BillDetails.BillDetailsID))
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

        private bool BillDetailsExists(int id)
        {
            return _context.BillDetails.Any(e => e.BillDetailsID == id);
        }
    }
}
