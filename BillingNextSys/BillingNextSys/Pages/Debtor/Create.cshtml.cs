using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BillingNextSys.Models;

namespace BillingNextSys.PagesDebtor
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
        ViewData["DebtorGroupID"] = new SelectList(_context.DebtorGroup, "DebtorGroupID", "DebtorGroupAddress");
            return Page();
        }

        [BindProperty]
        public Debtor Debtor { get; set; }

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