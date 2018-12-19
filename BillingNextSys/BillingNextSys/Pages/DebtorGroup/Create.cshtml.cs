using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BillingNextSys.Models;

namespace BillingNextSys.PagesDebtorGroup
{
    public class CreateModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public CreateModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["BranchID"] = new SelectList(_context.Branch, "BranchID", "BranchAddress");
            return Page();
        }

        [BindProperty]
        public DebtorGroup DebtorGroup { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.DebtorGroup.Add(DebtorGroup);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}