using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;

namespace BillingNextSys.PagesParticulars
{
    public class DeleteModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public DeleteModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Particulars Particulars { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Particulars = await _context.Particulars.FirstOrDefaultAsync(m => m.ParticularsID == id);

            if (Particulars == null)
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

            Particulars = await _context.Particulars.FindAsync(id);

            if (Particulars != null)
            {
                _context.Particulars.Remove(Particulars);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
