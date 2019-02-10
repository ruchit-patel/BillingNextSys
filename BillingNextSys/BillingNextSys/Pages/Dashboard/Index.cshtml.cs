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
    public class IndexModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly UserManager<BillingNextUser> userManager;

        private Task<BillingNextUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        public IndexModel(BillingNextSys.Models.BillingNextSysContext context, UserManager<BillingNextUser> userManager, IHttpContextAccessor httpContextAccessor)
        {

            this.userManager = userManager;

            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                int cid = (int)_session.GetInt32("Cid");
                int bid = (int)_session.GetInt32("Bid");

                var userrole = userManager.GetRolesAsync(await GetCurrentUserAsync());
            
                if(userrole.Result.FirstOrDefault()=="Admin")
                {
                    return RedirectToPage("/Dashboard/Admin");
                }

                return RedirectToPage("/Dashboard/User");
            }
            catch (InvalidOperationException)
            {
                return RedirectToPage("/Index");
            }
        }
    }
}
