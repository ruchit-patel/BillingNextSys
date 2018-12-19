using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;

namespace BillingNextSys.PagesReceived
{
    public class DeleteModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public DeleteModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Received Received { get; set; }

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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Received = await _context.Received.FindAsync(id);

            if (Received != null)
            {
                _context.Received.Remove(Received);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
