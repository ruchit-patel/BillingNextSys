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
    public class IndexModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public IndexModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public IList<Models.BillSeries> BillSeries { get;set; }

        public async Task OnGetAsync()
        {
            BillSeries = await _context.BillSeries.ToListAsync();
        }
    }
}
