using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BillingNextSys.Pages.Received
{
    [Authorize(Roles = "Admin,Accountant,Developer")]
    public class OutstandingModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public OutstandingModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public IList<Models.Received> Received { get; set; }

        public void OnGet()
        {
            Received = _context.Received.Where(a=>a.BillDetailsID.Equals(0)).Include(r => r.DebtorGroup)
                .Include(r => r.Company).ToList();
        }
    }
}
