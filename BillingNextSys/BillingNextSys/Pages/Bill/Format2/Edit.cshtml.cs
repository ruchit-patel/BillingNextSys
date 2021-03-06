﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BillingNextSys.Pages.Bill.Format2
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public EditModel(BillingNextSys.Models.BillingNextSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public Models.Bill Bill { get; set; }

        //public IList<Models.Company> Companies { get; set; }
        //public IList<Models.Branch> Branches { get; set; }
        public IList<Models.BillDetails> BillDetailss { get; set; }


        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Bill = await _context.Bill
                .Include(b => b.BillSeries)
                .Include(b => b.Company)
                .Include(b => b.Branch)
                .Include(b => b.DebtorGroup).FirstOrDefaultAsync(m => m.BillNumber == id);

            if (Bill == null)
            {
                return NotFound();
            }

            //int cid = (int)_session.GetInt32("Cid");
            //int bid = (int)_session.GetInt32("Bid");
            //Companies = _context.Company.Where(ab => ab.CompanyID.Equals(cid)).ToList();
            //Branches = _context.Branch.Where(a => a.BranchID.Equals(bid)).ToList();
            BillDetailss = _context.BillDetails.Where(a => a.BillNumber.Equals(id)).Include(b => b.Particulars).Include(c=>c.Debtor).ToList();

            return Page();
        }

     



        public IActionResult OnDeleteDeleteBillDetails(int id)
        {
            _context.BillDetails.Remove(_context.BillDetails.Find(id));
            _context.SaveChanges();


            return new JsonResult("Deleted Successfully! Remove Row.");
        }


        public IActionResult OnPutUpdateBillDetails(int id, double damt, [FromBody]Models.BillDetails obj)
        {

            _context.Attach(obj).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
                var DebtorGroupOut = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(obj.DebtorGroupID)).Select(a => a.DebtorOutstanding).FirstOrDefault();

                var billout = DebtorGroupOut + damt;
                var dgout = new Models.DebtorGroup { DebtorGroupID = obj.DebtorGroupID, DebtorOutstanding = billout };
                _context.DebtorGroup.Attach(dgout).Property(x => x.DebtorOutstanding).IsModified = true;
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillDetailsExists(obj.BillDetailsID))
                {
                    return new JsonResult("Update Error!");
                }
                else
                {
                    return new JsonResult("Update Error!");
                }
            }
            return new JsonResult("Information Updated!");
        }
        private bool BillDetailsExists(int id)
        {
            return _context.BillDetails.Any(e => e.BillDetailsID == id);
        }


        public IActionResult OnPutUpdateBill2(int id, [FromBody]Models.Bill obj)
        {
          

            _context.Attach(obj).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillExists(obj.BillNumber))
                {
                    return new JsonResult("Update Error!");
                }
                else
                {
                    return new JsonResult("Update Error!");
                }
            }
            return new JsonResult("Bill Information Updated!");
        }
        private bool BillExists(string id)
        {
            return _context.Bill.Any(e => e.BillNumber == id);
        }

        public IActionResult OnGetUpdateBillAmt(string bnum, double billamt)
        {
            try
            {
                if (Double.IsNaN(billamt))
                {
                    return NotFound();
                }
                var billamtobj = new Models.Bill { BillNumber = bnum, BillAmount = billamt };
                _context.Bill.Attach(billamtobj).Property(x => x.BillAmount).IsModified = true;
                _context.SaveChanges();

                return new JsonResult("Information Updated!");
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

    }
}
