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

namespace BillingNextSys.Pages.Bill.Format1
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

            lastbillnumber = _context.Bill.OrderByDescending(a => a.BillDate).Select(c => c.BillNumber).FirstOrDefault().ToString();

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

        public IActionResult OnGetBillTo(string str)
        {
            List<Models.DebtorGroup> data = _context.DebtorGroup.Where(a => a.DebtorGroupName.ToLower().Contains(str.ToLower())).ToList();
            return new JsonResult(data);
        }

        public IActionResult OnGetBillToDetails(int id)
        {
           Models.DebtorGroup data = _context.DebtorGroup.Find(id);
            return new JsonResult(data);
        }

        public IActionResult OnGetSeries()
        {
            List<Models.BillSeries> data = _context.BillSeries.ToList();
            return new JsonResult(data);
        }

        public IActionResult OnGetParticulars(string str)
        {
            List<Models.Particulars> data = _context.Particulars.Where(a => a.ParticularsName.ToLower().Contains(str.ToLower())).ToList();
            return new JsonResult(data);
        }

        public IActionResult OnGetParticularsDetails(int id)
        {
            Models.Particulars data = _context.Particulars.Find(id);
            return new JsonResult(data);
        }

        public IActionResult OnPostInsertBillDetails([FromBody] Models.BillDetails obj)
        {
            obj.CompanyID= (int)_session.GetInt32("Cid");
            _context.BillDetails.Add(obj);
            _context.SaveChanges();
            return new JsonResult("Added Successfully!");
        }

        public IActionResult OnPostInsertBill([FromBody] Models.Bill obj)
        {
            obj.CompanyID = (int)_session.GetInt32("Cid");
            obj.BillDate = DateTime.Now;
            _context.Bill.Add(obj);
            _context.SaveChanges();
            return new JsonResult("Added Successfully!");
        }
    }
}