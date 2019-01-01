using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using Npgsql;
using System.Data.Common;

namespace BillingNextSys.Pages.DebtorGroup
{
    public class IndexModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public IndexModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.DebtorGroup DebtorGroup { get; set; }

        [BindProperty]
        public Models.Debtor Debtor { get; set; }

        public IList<Models.DebtorGroup> DebtorGroups { get;set; }

        public async Task OnGetAsync()
        {
            ViewData["BranchID"] = new SelectList(_context.Branch, "BranchID", "BranchName");

            DebtorGroups = await _context.DebtorGroup
                .Include(d => d.Branch).ToListAsync();


        }
        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    _context.DebtorGroup.Add(DebtorGroup);
        //    await _context.SaveChangesAsync();

        //    return RedirectToPage("./Index");
        //}

        public IActionResult OnPostInsert([FromBody] Models.DebtorGroup obj)
        {
            //return new JsonResult("Customer Added Successfully!");
            _context.DebtorGroup.Add(obj);
            _context.SaveChanges();

            var result = _context.AdHocReturns.FromSql("SELECT last_value FROM \"DebtorGroup_DebtorGroupID_seq\"").FirstOrDefault().last_value;

            return new JsonResult("Debtor Added Successfully!"+result);
        }
        public IActionResult OnPostInsertD([FromBody] Models.Debtor obj)
        {
            //return new JsonResult("Customer Added Successfully!");
            _context.Debtor.Add(obj);
            _context.SaveChanges();
            return new JsonResult("Individual Debtor Added Successfully!");
        }
    }
}
