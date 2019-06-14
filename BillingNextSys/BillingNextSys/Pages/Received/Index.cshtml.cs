using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;
using Microsoft.AspNetCore.Authorization;

namespace BillingNextSys.Pages.Received
{
    [Authorize(Roles = "Admin,Accountant,Developer")]
    public class IndexModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public IndexModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public IList<Models.Received> Received { get;set; }

        public void OnGet(int companyid)
        {
            Received = _context.Received.Where(a=>a.CompanyID.Equals(companyid))
                .Include(r=>r.BillDetails)
                .Include(r=>r.DebtorGroup)
                .Include(r => r.Company).ToList();
        }

    }
}


