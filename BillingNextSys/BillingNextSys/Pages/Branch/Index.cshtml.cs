using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace BillingNextSys.Pages.Branch
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public IndexModel(BillingNextSys.Models.BillingNextSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IList<Models.Branch> Branch { get;set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //if (_session.GetInt32("Cid").HasValue)
            //{
            //    Branch = await _context.Branch
            // .Include(b => b.Company).Where(b => b.Company.CompanyID == _session.GetInt32("Cid")).ToListAsync();
            //    return Page();
            //}
            if (id==null)
            {
                Branch = await _context.Branch
                .Include(b => b.Company).ToListAsync();
                return Page();
            }
         
           Branch = await _context.Branch
                .Include(b => b.Company).Where(b=> b.Company.CompanyID==id).ToListAsync();
            if (Branch.Any())
            {
                return Page();
            }
            else
            {
                return RedirectToPage("./Create");
            }
        }
    }
}
