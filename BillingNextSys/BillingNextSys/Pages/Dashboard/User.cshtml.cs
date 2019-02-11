using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillingNextSys.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BillingNextSys.Pages.Dashboard
{
    [Authorize]
    public class UserModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public UserModel(BillingNextSys.Models.BillingNextSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IList<Models.Company> Companies { get; set; }
        public string branchname { get; set; }
        public IActionResult OnGet()
        {
            try
            {
                int cid = (int)_session.GetInt32("Cid");
                int bid = (int)_session.GetInt32("Bid");

                Companies = _context.Company.Where(a => a.CompanyID.Equals(cid)).ToList();
                branchname = _context.Branch.Where(a => a.BranchID.Equals(bid)).Select(a => a.BranchName).FirstOrDefault().ToString();
                return Page();
            }
            catch (InvalidOperationException)
            {
                return RedirectToPage("/Index");
            }
        }
    }
}
