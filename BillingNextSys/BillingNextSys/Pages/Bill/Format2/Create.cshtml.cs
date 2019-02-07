using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BillingNextSys.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BillingNextSys.Pages.Bill.Format2
{
    public class CreateModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public CreateModel(BillingNextSys.Models.BillingNextSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult OnGet()
        {
        ViewData["SeriesName"] = new SelectList(_context.BillSeries, "SeriesName", "SeriesName");
        ViewData["CompanyID"] = new SelectList(_context.Company, "CompanyID", "CompanyName");
        ViewData["DebtorGroupID"] = new SelectList(_context.DebtorGroup, "DebtorGroupID", "DebtorGroupName");
            int cid = (int)_session.GetInt32("Cid");
            int bid = (int)_session.GetInt32("Bid");
            Companies = _context.Company.Where(ab => ab.CompanyID.Equals(cid)).ToList();
            Branches = _context.Branch.Where(a => a.BranchID.Equals(bid)).ToList();

            lastbillnumber = _context.Bill.Where(ab=> !(ab.BillActNum.HasValue)).OrderByDescending(a => a.BillDate).Select(c => c.BillNumber).FirstOrDefault().ToString();

            return Page();
        }

        [BindProperty]
        public Models.Bill Bill { get; set; }

        public string lastbillnumber=null;
        public IList<Models.Company> Companies { get; set; }
        public IList<Models.Branch> Branches{ get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Bill.Add(Bill);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }



        public IActionResult OnGetBillToDebtorInfo(int id)
        {
            List<Models.Debtor> data = _context.Debtor.Where(a => a.DebtorGroupID.Equals(id)).ToList();
            return new JsonResult(data);
        }



        public IActionResult OnPostInsertBillDetails([FromBody] Models.BillDetails obj)
        {
            obj.CompanyID= (int)_session.GetInt32("Cid");
            obj.BillAmountOutstanding = obj.Amount;
            _context.BillDetails.Add(obj);
            _context.SaveChanges();
            return new JsonResult("Added Successfully!");
        }

        public IActionResult OnPostInsertBill([FromBody] Models.Bill obj)
        {
            try
            {
                obj.CompanyID = (int)_session.GetInt32("Cid");
                obj.BranchID = (int)_session.GetInt32("Bid");
                Random generator = new Random();
                obj.SecretUnlockCode = Int32.Parse(generator.Next(0, 999999).ToString("D6"));
                obj.BillDelivered = false;
                obj.MessageSent = false;
                obj.BillDate = DateTime.Now;
                _context.Bill.Add(obj);
                _context.SaveChanges();
                return new JsonResult("Added Successfully!");
            }
            catch(DbUpdateException exception)
            {
                string exce= "Update 1";
                return new JsonResult(exce);
            }
        }
    }
}