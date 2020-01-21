using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BillingNextSys.Models;

namespace BillingNextSys.Pages.AdvancePay
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
        ViewData["BranchID"] = new SelectList(_context.Branch, "BranchID", "BranchName");
        ViewData["CompanyID"] = new SelectList(_context.Company, "CompanyID", "CompanyName");
        ViewData["DebtorGroupID"] = new SelectList(_context.DebtorGroup, "DebtorGroupID", "DebtorGroupName");
            return Page();
        }

        [BindProperty]
        public Models.AdvancePay AdvancePay { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.AdvancePay.Add(AdvancePay);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}