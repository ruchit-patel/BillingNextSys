using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BillingNextSys.Pages.Branch
{
    public class SelectBranchModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;


        public SelectBranchModel(BillingNextSys.Models.BillingNextSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        public IActionResult OnGet()
        {
            if (_session.GetInt32("Cid").HasValue)
            {
                ViewData["BranchList"] = new SelectList(_context.Branch.Include(b => b.Company).Where(b => b.Company.CompanyID == _session.GetInt32("Cid")), "BranchID", "BranchName");
                return Page();
            }
            return RedirectToPage("/Index");
        }

        public IActionResult OnPost()
        {
            int Bid = Convert.ToInt32(Request.Form["Bid"].ToString());
            HttpContext.Session.SetInt32("Bid", Bid);
            return RedirectToPage("/Debtor/Index");
        }
    }
}
