using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BillingNextSys.Models;

namespace BillingNextSys.PagesBranch
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
        ViewData["CompanyID"] = new SelectList(_context.Company, "CompanyID", "AccountNumber");
            return Page();
        }

        [BindProperty]
        public Branch Branch { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Branch.Add(Branch);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}