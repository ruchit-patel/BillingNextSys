using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;

namespace BillingNextSys.PagesBillSeries
{
    public class DeleteModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public DeleteModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BillSeries BillSeries { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BillSeries = await _context.BillSeries.FirstOrDefaultAsync(m => m.SeriesName == id);

            if (BillSeries == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BillSeries = await _context.BillSeries.FindAsync(id);

            if (BillSeries != null)
            {
                _context.BillSeries.Remove(BillSeries);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
