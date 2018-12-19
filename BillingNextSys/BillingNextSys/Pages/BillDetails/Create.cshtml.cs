using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BillingNextSys.Models;

namespace BillingNextSys.Pages.BillDetails
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
        ViewData["BillNumber"] = new SelectList(_context.Bill, "BillNumber", "BillNumber");
        ViewData["CompanyID"] = new SelectList(_context.Company, "CompanyID", "CompanyName");
        ViewData["DebtorID"] = new SelectList(_context.Debtor, "DebtorID", "DebtorName");
        ViewData["DebtorGroupID"] = new SelectList(_context.DebtorGroup, "DebtorGroupID", "DebtorGroupName");
        ViewData["ParticularsID"] = new SelectList(_context.Particulars, "ParticularsID", "ParticularsName");
            return Page();
        }

        [BindProperty]
        public Models.BillDetails BillDetails { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BillDetails.Add(BillDetails);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}