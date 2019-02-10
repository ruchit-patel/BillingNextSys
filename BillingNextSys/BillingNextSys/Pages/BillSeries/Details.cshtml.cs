using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;
using Microsoft.AspNetCore.Authorization;

namespace BillingNextSys.Pages.BillSeries
{
    [Authorize(Roles = "Admin,Developer")]
    public class DetailsModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public DetailsModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public Models.BillSeries BillSeries { get; set; }

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
    }
}
