using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillingNextSys.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BillingNextSys.Pages.Dashboard
{
    public class AdminModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        private readonly UserManager<BillingNextUser> userManager;
        private readonly BillingNextSysIdentityDbContext _idcontext;
        private readonly RoleManager<IdentityRole> roleManager;


        public AdminModel(BillingNextSys.Models.BillingNextSysContext context, UserManager<BillingNextUser> userManager, BillingNextSysIdentityDbContext idcontext, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _idcontext = idcontext;
            _context = context;
        }


        public IEnumerable<BillingNextUser> usersofRoleAdmin;
        public IEnumerable<BillingNextUser> usersofRoleAccountant;
        public IEnumerable<BillingNextUser> usersofRoleDeveloper;


        public IQueryable<IdentityRole> Roles;


        public IList<Models.Company> Company { get; set; }

        public async Task OnGetAsync()
        {
            usersofRoleAdmin = await userManager.GetUsersInRoleAsync("Admin");
            usersofRoleAccountant = await userManager.GetUsersInRoleAsync("Accountant");
            usersofRoleDeveloper = await userManager.GetUsersInRoleAsync("Developer");
             Roles = roleManager.Roles;
            Company = await _context.Company.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string uRole = Request.Form["Roles"].ToString();
            string user = Request.Form["UserID"].ToString();
            string curRole = Request.Form["curRole"].ToString();
            BillingNextUser userObj;
            userObj = await userManager.FindByIdAsync(user);
            IdentityResult IR1, IR2;
            IR2 = await userManager.RemoveFromRoleAsync(userObj, curRole);
            IR1 = await userManager.AddToRoleAsync(userObj, uRole);
            if (IR1.Succeeded && IR2.Succeeded)
            {
                return RedirectToPage("./Admin");
            }
           
                return RedirectToPage("../Error");
     
        }
    }
}
