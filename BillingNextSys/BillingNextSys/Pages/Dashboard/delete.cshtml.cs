using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillingNextSys.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BillingNextSys.Pages.Dashboard
{
    [Authorize(Roles = "Admin")]
    public class deleteModel : PageModel
    {
        private readonly UserManager<BillingNextUser> userManager;

        public deleteModel(UserManager<BillingNextUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            BillingNextUser userObj;
            userObj = await userManager.FindByIdAsync(id);
            IdentityResult IR1;

            IR1 = await userManager.DeleteAsync(userObj);

            if (IR1.Succeeded)
            {
                return RedirectToPage("./Admin");
            }
            else
            {
                return RedirectToPage("../Error");
            }
        }
    }
}
