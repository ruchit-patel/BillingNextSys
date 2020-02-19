using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BillingNextSys.Pages.AdvancePay
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public IndexModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }
    }


    [Authorize]
    public class IndexGridModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public IndexGridModel(BillingNextSys.Models.BillingNextSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IQueryable<Models.AdvancePay> AdvancePays { get; set; }

        public IActionResult OnGet()
        {
            try
            {
                int cid = (int)_session.GetInt32("Cid");
                int bid = (int)_session.GetInt32("Bid");
             var AdvancePays =  _context.AdvancePay.Join(
                    _context.Company,
                       bb => bb.CompanyID,
                       cc => cc.CompanyID,
                       (bb, cc) => new
                       {
                           cc.CompanyID,
                           cc.CompanyName,
                           bb.AdvanceAmount,
                           bb.ChequePaymet,
                           bb.ChequeNumber,
                           bb.ReceivedDate,
                           bb.DebtorGroupID,
                           bb.BranchID
                       }).Join(
                 _context.Branch,
                    bb => bb.BranchID,
                    cc => cc.BranchID,
                    (bb, cc) => new
                    {
                        cc.BranchID,
                        cc.BranchName,
                        bb.AdvanceAmount,
                        bb.ChequePaymet,
                        bb.ChequeNumber,
                        bb.ReceivedDate,
                        bb.CompanyName,
                        bb.DebtorGroupID
                    }).Join(
                    _context.DebtorGroup,
                        bb => bb.DebtorGroupID,
                        dg => dg.DebtorGroupID,
                        (bb, dg) => new
                        {
                            dg.DebtorGroupName,
                            dg.DebtorGroupID,
                            bb.ChequePaymet,
                            bb.ChequeNumber,
                            bb.ReceivedDate,
                            bb.AdvanceAmount
                        }
                    );
                      
                
                return Page();
            }
            catch (InvalidOperationException)
            {
                return RedirectToPage("/Index");
            }
        }
    }
}
