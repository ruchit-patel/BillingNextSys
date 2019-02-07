using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BillingNextSys.Pages.Bill.Format1
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

        public async Task<IActionResult> OnGetSelectAllAsync(string id)
        {
            List<Models.BillDetails> data = await _context.BillDetails.Where(ab=> ab.BillNumber.Equals(id)).ToListAsync();
            return new JsonResult(data);
        }

        public IActionResult OnPostInsertReceived(int dgid,[FromBody] Models.Received obj)
        { 
            obj.CompanyID= (int)_session.GetInt32("Cid");
            _context.Received.Add(obj);
            _context.SaveChanges();

            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var amtout = _context.BillDetails.Where(a => a.BillDetailsID.Equals(obj.BillDetailsID)).FirstOrDefault().BillAmountOutstanding;
            // var amtout = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(dgid)).FirstOrDefault().DebtorOutstanding;
            var billout = amtout - obj.ReceivedAmount;
            var billdet = new Models.BillDetails { BillDetailsID = obj.BillDetailsID, BillAmountOutstanding= billout };
            _context.BillDetails.Attach(billdet).Property(x => x.BillAmountOutstanding).IsModified = true;
            _context.SaveChanges();

            var dgout = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(dgid)).FirstOrDefault().DebtorOutstanding;

            var debout = dgout - obj.ReceivedAmount;
            var dgdet = new Models.DebtorGroup { DebtorGroupID = dgid, DebtorOutstanding = debout };
            _context.DebtorGroup.Attach(dgdet).Property(x => x.DebtorOutstanding).IsModified = true;
            _context.SaveChanges();

            return new JsonResult("Successful!");
        }

        //public IActionResult OnPutUpdateReceived(int id, [FromBody]Models.Received obj)
        //{
        //    _context.Attach(obj).State = EntityState.Modified;

        //    try
        //    {
        //        _context.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!DebtorExists(obj.BillNumber))
        //        {

        //            return new JsonResult("Debtor Update Error!");
        //        }
        //        else
        //        {
        //            return new JsonResult("Debtor Update Error!");
        //        }
        //    }
        //    return new JsonResult("Debtor Information Updated!");
        //}
        //private bool DebtorExists(string id)
        //{
        //    return _context.Bill.Any(e => e.BillNumber == id);
        //}
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
            Bills = _context.Bill.Where(a => a.BillActNum.HasValue).AsQueryable();
        }
    }

}
