using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;

namespace BillingNextSys.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public IndexModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["CompanyList"] = new SelectList(_context.Company, "CompanyID", "CompanyName");
            return Page();
        }
        public IActionResult OnPost()
        {
            int Cid = Convert.ToInt32(Request.Form["Cid"].ToString());
            HttpContext.Session.SetInt32("Cid", Cid);
            return RedirectToPage("/Branch/SelectBranch");
        }
    }
}
