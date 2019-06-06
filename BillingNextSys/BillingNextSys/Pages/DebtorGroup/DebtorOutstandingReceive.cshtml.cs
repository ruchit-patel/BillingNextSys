using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BillingNextSys.Pages.DebtorGroup
{
    [Authorize]
    public class DebtorOutstandingReceiveModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public DebtorOutstandingReceiveModel(BillingNextSys.Models.BillingNextSysContext context,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public string debtorgroupname;
        public int debtorgid;
        public double debtoroutstandinginit;
        public IActionResult OnGet(string debtorgname, int debtorid)
        {
            debtorgroupname = debtorgname;
            debtorgid = debtorid;
           double debout= _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(debtorid)).Select(a => a.DebtorOutstanding).FirstOrDefault();
            double debbillout = _context.BillDetails.Where(a => a.DebtorGroupID.Equals(debtorid)).Sum(a => a.BillAmountOutstanding);
            debtoroutstandinginit = debout - debbillout;
            return Page();
        }

        [BindProperty]
        public Models.Received Received { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                Received.CompanyID = (int)_session.GetInt32("Cid");

                _context.Received.Add(Received);
                await _context.SaveChangesAsync();


                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var DebtorGroupOut = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(Received.DebtorGroupID)).Select(a => a.DebtorOutstanding).FirstOrDefault();

                var debtorrec = DebtorGroupOut - Received.ReceivedAmount;
                var dgout = new Models.DebtorGroup { DebtorGroupID = Received.DebtorGroupID, DebtorOutstanding = debtorrec };
                _context.DebtorGroup.Attach(dgout).Property(x => x.DebtorOutstanding).IsModified = true;
                _context.SaveChanges();
                return RedirectToPage("./Index");
            }
            catch(Exception)
            {
                return Page();
            }
        }
    }
}
