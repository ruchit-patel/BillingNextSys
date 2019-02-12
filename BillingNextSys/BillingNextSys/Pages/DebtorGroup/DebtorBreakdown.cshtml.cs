using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BillingNextSys.Pages.DebtorGroup
{
    [Authorize]
    public class DebtorBreakdownModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public DebtorBreakdownModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public IQueryable<Models.BillDetails> BillsDetailss { get; set; }

        public double Detamt;


        public void OnGet(int debid)
        {
            BillsDetailss = _context.BillDetails.Where(ab => ab.DebtorID.Equals(debid)).Include(a=>a.Debtor).AsQueryable();
            Detamt = _context.BillDetails.Where(a => a.DebtorID.Equals(debid)).Sum(a => a.BillAmountOutstanding);
           
        }
    }
}
