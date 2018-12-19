using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BillingNextSys.Models;

namespace BillingNextSys.Pages.Bill
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
        ViewData["SeriesName"] = new SelectList(_context.BillSeries, "SeriesName", "SeriesName");
        ViewData["CompanyID"] = new SelectList(_context.Company, "CompanyID", "CompanyName");
        ViewData["DebtorGroupID"] = new SelectList(_context.DebtorGroup, "DebtorGroupID", "DebtorGroupName");
            return Page();
        }

        [BindProperty]
        public Models.Bill Bill { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Bill.Add(Bill);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}