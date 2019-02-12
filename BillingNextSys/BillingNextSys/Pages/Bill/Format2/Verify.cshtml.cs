using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BillingNextSys.Pages.Bill.Format2
{
    public class VerifyModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public VerifyModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public string billnum;
        public IActionResult OnGet(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            billnum = id;
            return Page();
        }
        public IActionResult OnPost(int num, string billnum)
        {
           var rowcn =_context.Bill.Where(a => a.BillNumber == billnum && a.SecretUnlockCode == num).Select(a => a.BillNumber).FirstOrDefault();

            if(string.IsNullOrEmpty(rowcn))
            {
                return NotFound();
            }
            else
            {
                var billdelivered = new Models.Bill { BillNumber = billnum, BillDelivered = true };
                _context.Bill.Attach(billdelivered).Property(x => x.BillDelivered).IsModified = true;
                _context.SaveChanges();

                return RedirectToPage("/Bill/Format2/PrintGuest",new { id=billnum, seccode =num});
            }

        }
    }
}
