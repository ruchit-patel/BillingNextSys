using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BillingNextSys.Models;

namespace BillingNextSys.PagesReceived
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
        ViewData["BillDetailsID"] = new SelectList(_context.BillDetails, "BillDetailsID", "ParticularsName");
        ViewData["CompanyID"] = new SelectList(_context.Company, "CompanyID", "AccountNumber");
            return Page();
        }

        [BindProperty]
        public Received Received { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Received.Add(Received);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}