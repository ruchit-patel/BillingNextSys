using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BillingNextSys.Models;
using Microsoft.AspNetCore.Authorization;

namespace BillingNextSys.Pages.Debtor
{
    [Authorize(Roles = "Developer")]
    public class CreateModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public CreateModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["DebtorGroupID"] = new SelectList(_context.DebtorGroup, "DebtorGroupID", "DebtorGroupName");
            return Page();
        }

        [BindProperty]
        public Models.Debtor Debtor { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Debtor.Add(Debtor);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}