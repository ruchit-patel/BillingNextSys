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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using BillingNextSys.Hubs;
using BillingNextSys.Services;
using BillingNextSys.DataModels;
using Newtonsoft.Json;

namespace BillingNextSys.Pages.Dashboard
{
    [Authorize(Roles = "Admin")]
    public class AdminModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        private readonly UserManager<BillingNextUser> userManager;
        private readonly BillingNextSysIdentityDbContext _idcontext;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IGraphDataService _graphDataService;
        

        public int CompanyId;

        public AdminModel(BillingNextSys.Models.BillingNextSysContext context, UserManager<BillingNextUser> userManager, BillingNextSysIdentityDbContext idcontext, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor, IGraphDataService graphDataService)
        {
      
            this.userManager = userManager;
            this.roleManager = roleManager;
            _idcontext = idcontext;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _graphDataService = graphDataService;
        }


        public IEnumerable<BillingNextUser> usersofRoleAdmin;
        public IEnumerable<BillingNextUser> usersofRoleAccountant;
        public IEnumerable<BillingNextUser> usersofRoleDeveloper;


        public IQueryable<IdentityRole> Roles;

        public IList<Models.Company> Companydet { get; set; }
        public IList<Models.Company> Company { get; set; }
        public string branchname;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                int cid = (int)_session.GetInt32("Cid");
                int bid = (int)_session.GetInt32("Bid");
                CompanyId = cid;
                usersofRoleAdmin = await userManager.GetUsersInRoleAsync("Admin");
                usersofRoleAccountant = await userManager.GetUsersInRoleAsync("Accountant");
                usersofRoleDeveloper = await userManager.GetUsersInRoleAsync("Developer");
                Roles = roleManager.Roles;
                Company = await _context.Company.ToListAsync();
                Companydet = _context.Company.Where(a => a.CompanyID.Equals(cid)).ToList();
                branchname = _context.Branch.Where(a => a.BranchID.Equals(bid)).Select(a => a.BranchName).FirstOrDefault().ToString();
                return Page();
            }
            catch (InvalidOperationException)
            {
                return RedirectToPage("/Index");
            }


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

        public async Task<IActionResult> OnPostUpdateGraph( string type)
        {
            if (type == "all")
            {
                await _graphDataService.UpdateCashFlowsGraph();
            }
            else if(type=="CashFlows")
            {
                await _graphDataService.UpdateCashFlowsGraph();
            }

            return new JsonResult("Request made to update "+type+" graph(s).");
        }
    }
}
