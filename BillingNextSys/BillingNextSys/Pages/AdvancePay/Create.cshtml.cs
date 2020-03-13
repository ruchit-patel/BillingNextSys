﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NonFactors.Mvc.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BillingNextSys.Pages.AdvancePay
{
    public class CreateModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public CreateModel(BillingNextSys.Models.BillingNextSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult OnGet()
        {
            ViewData["BranchID"] = new SelectList(_context.Branch, "BranchID", "BranchName");
            ViewData["CompanyID"] = new SelectList(_context.Company, "CompanyID", "CompanyName");
            return Page();
        }
        
        public JsonResult OnGetAllDebtorGroups(LookupFilter filter)
        {
            DebtorGroupLookup lookup = new DebtorGroupLookup(_context) { Filter = filter };
            return new JsonResult(lookup.GetData());
        }


        [BindProperty]
        public Models.AdvancePay AdvancePay { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            AdvancePay.CompanyID = (int)_session.GetInt32("Cid");
            AdvancePay.BranchID = (int)_session.GetInt32("Bid");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.AdvancePay.Add(AdvancePay);
            await _context.SaveChangesAsync();

            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var dgout = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(AdvancePay.DebtorGroupID)).Select(x=> new { x.DebtorOutstanding, x.AdvancePayAmount}).FirstOrDefault();


            var debtorgroup = _context.DebtorGroup.Find(AdvancePay.DebtorGroupID);
            debtorgroup.DebtorOutstanding = dgout.DebtorOutstanding - AdvancePay.AdvanceAmount;
            debtorgroup.AdvancePayAmount = dgout.AdvancePayAmount + AdvancePay.AdvanceAmount;

            _context.Entry(debtorgroup).State = EntityState.Modified;
            await _context.SaveChangesAsync();


            return RedirectToPage("./Index");
        }
    }
    public class DebtorGroupLookup : MvcLookup<Models.DebtorGroup>
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public DebtorGroupLookup(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
            GetId = (model) => model.DebtorGroupID.ToString();
        }
        public DebtorGroupLookup()
        {
            Url = "Create?handler=AllDebtorGroups";
            Title = "Debtor Group";
            Filter.Order = LookupSortOrder.Desc;
        }

        public override IQueryable<Models.DebtorGroup> GetModels()
        {
            return _context.Set<Models.DebtorGroup>();
        }

    } 
}