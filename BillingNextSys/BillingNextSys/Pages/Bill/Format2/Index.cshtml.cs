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
    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public IndexModel(BillingNextSys.Models.BillingNextSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public void OnGet()
        {
        }

        public IActionResult OnGetSelectAll(string id)
        {
            var query = from billdetails in _context.BillDetails
                        join debtor in _context.Debtor on billdetails.DebtorID equals debtor.DebtorID into gj
                        from subset in gj.DefaultIfEmpty()
                        where billdetails.BillNumber.Equals(id)
                        select new { billdetails, DebtorName = subset.DebtorName ?? String.Empty };


            return new JsonResult(query);
        }
    }
    public class IndexGridModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public IndexGridModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }
        public IQueryable<Models.Bill> Bills { get; set; }

        public void OnGet()
        {
            Bills = _context.Bill.Where(ab => !(ab.BillActNum.HasValue)).AsQueryable();
        }
    }
}
