using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;

namespace BillingNextSys.Pages.Company
{
    public class IndexModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public IndexModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public IList<Models.Company> Company { get;set; }

        public async Task OnGetAsync()
        {
            Company = await _context.Company.ToListAsync();
        }
    }
}
