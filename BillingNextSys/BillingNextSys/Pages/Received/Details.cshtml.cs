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
    public class DetailsModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public DetailsModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

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
    }
}
