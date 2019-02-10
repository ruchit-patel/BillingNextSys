using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BillingNextSys.Pages.DebtorGroup
{
    [Authorize]
    public class OutstandingBreakDownModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public OutstandingBreakDownModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public IQueryable<Models.Bill> Bills { get; set; }

        public double BillAmt;
        public double DebtorOutstand;

        public void OnGet(int debid)
        {
            Bills = _context.Bill.Where(ab => ab.DebtorGroupID.Equals(debid)).AsQueryable();
            DebtorOutstand = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(debid)).Select(ab => ab.DebtorOutstanding).FirstOrDefault();
            BillAmt = _context.Bill.Where(a=>a.DebtorGroupID.Equals(debid)).Sum(a => a.BillAmount);

        }
    }
}
