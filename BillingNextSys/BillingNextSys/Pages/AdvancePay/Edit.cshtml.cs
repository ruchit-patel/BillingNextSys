using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;

namespace BillingNextSys.Pages.AdvancePay
{
    public class EditModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public EditModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           ViewData["BranchID"] = new SelectList(_context.Branch, "BranchID", "BranchName");
           ViewData["CompanyID"] = new SelectList(_context.Company, "CompanyID", "CompanyName");
           ViewData["DebtorGroupID"] = new SelectList(_context.DebtorGroup, "DebtorGroupID", "DebtorGroupName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(AdvancePay).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdvancePayExists(AdvancePay.AdvancePayID))
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

        private bool AdvancePayExists(int id)
        {
            return _context.AdvancePay.Any(e => e.AdvancePayID == id);
        }
    }
}
