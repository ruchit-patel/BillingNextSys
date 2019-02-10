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
using Microsoft.AspNetCore.Authorization;

namespace BillingNextSys.Pages.DebtorGroup
{
    [Authorize]
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
        public IList<Models.Debtor> Debtors { get; set; }

        public async Task OnGetAsync()
        {
            ViewData["BranchID"] = new SelectList(_context.Branch, "BranchID", "BranchName");

            DebtorGroups = await _context.DebtorGroup
                .Include(d => d.Branch).ToListAsync();

            Debtors = await _context.Debtor.ToListAsync();
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
        public IActionResult OnGetSelectAll(int id)
        {
            List<Models.Debtor> data = _context.Debtor.Where(a => a.DebtorGroupID.Equals(id)).ToList();
            return new JsonResult(data);
        }

        public IActionResult OnGetAllDebtorG()
        {
            List<Models.DebtorGroup> data = _context.DebtorGroup.ToList();
            return new JsonResult(data);
        }

        public IActionResult OnDeleteDeleteDebtor(int id)
        {
            Debtor = _context.Debtor.Find(id);

            if (Debtor != null)
            {
                _context.Debtor.Remove(Debtor);
                 _context.SaveChangesAsync();
                return new JsonResult("Deleted Success");
            }
            return new JsonResult("Delete Unsuccess");
        }

        public IActionResult OnPutUpdate(int id, [FromBody]Models.Debtor obj)
        {
            _context.Attach(obj).State = EntityState.Modified;

            try
            {
                 _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DebtorExists(obj.DebtorID))
                {
             
                    return new JsonResult("Debtor Update Error!");
                }
                else
                {
                    return new JsonResult("Debtor Update Error!");
                }
            }
            return new JsonResult("Debtor Information Updated!");
        }
        private bool DebtorExists(int id)
        {
            return _context.Debtor.Any(e => e.DebtorID == id);
        }

    }
}
