using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;

namespace BillingNextSys.Pages.Received
{
    public class EditModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public EditModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Received Received { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Received = await _context.Received
                .Include(r => r.BillDetails)
                .Include(r => r.Company).FirstOrDefaultAsync(m => m.ReceivedID == id);

            if (Received == null)
            {
                return NotFound();
            }
           ViewData["BillDetailsID"] = new SelectList(_context.BillDetails, "BillDetailsID", "ParticularsName");
           ViewData["CompanyID"] = new SelectList(_context.Company, "CompanyID", "CompanyName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Received).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReceivedExists(Received.ReceivedID))
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

        private bool ReceivedExists(int id)
        {
            return _context.Received.Any(e => e.ReceivedID == id);
        }
    }
}
