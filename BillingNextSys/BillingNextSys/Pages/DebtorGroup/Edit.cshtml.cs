using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;

namespace BillingNextSys.Pages.DebtorGroup
{
    public class EditModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public EditModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.DebtorGroup DebtorGroup { get; set; }

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
           ViewData["BranchID"] = new SelectList(_context.Branch, "BranchID", "BranchAddress");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(DebtorGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DebtorGroupExists(DebtorGroup.DebtorGroupID))
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

        private bool DebtorGroupExists(int id)
        {
            return _context.DebtorGroup.Any(e => e.DebtorGroupID == id);
        }
    }
}
