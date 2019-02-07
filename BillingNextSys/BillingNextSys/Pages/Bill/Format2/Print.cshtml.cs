using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BillingNextSys.Pages.Bill.Format2
{
    public class PrintModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public PrintModel(BillingNextSys.Models.BillingNextSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public Models.Bill Bill { get; set; }

     
        public IList<Models.BillDetails> BillDetailss { get; set; }


        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Bill = await _context.Bill
                .Include(b => b.BillSeries)
                .Include(b => b.Company)
                .Include(b => b.Branch)
                .Include(b => b.DebtorGroup).FirstOrDefaultAsync(m => m.BillNumber == id);

            if (Bill == null)
            {
                return NotFound();
            }

           
            BillDetailss = _context.BillDetails.Where(a => a.BillNumber.Equals(id)).Include(b => b.Particulars).Include(c => c.Debtor).ToList();

            return Page();
        }
    }
}
