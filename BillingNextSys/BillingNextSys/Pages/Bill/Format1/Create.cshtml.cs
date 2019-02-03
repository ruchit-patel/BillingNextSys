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
using BillingNextSys.Services;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BillingNextSys.Pages.Bill.Format1
{
    public class CreateModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly BillingNextSys.Models.BillingNextSysContext _context;
        private readonly IWhatsappSender _smsSender;

        public CreateModel(BillingNextSys.Models.BillingNextSysContext context, IHttpContextAccessor httpContextAccessor, IOptions<Whatsappoptions> optionsAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            Options = optionsAccessor.Value;
        }
        public Whatsappoptions Options { get; }

        public IActionResult OnGet()
        {
        
            int cid = (int)_session.GetInt32("Cid");
            int bid = (int)_session.GetInt32("Bid");
            Companies = _context.Company.Where(ab => ab.CompanyID.Equals(cid)).ToList();
            Branches = _context.Branch.Where(a => a.BranchID.Equals(bid)).ToList();

            lastbillnumber = _context.Bill.Where(ab=>ab.BillActNum.HasValue).OrderByDescending(a => a.BillDate).Select(c => c.BillNumber).FirstOrDefault().ToString();

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
            try
            {
                obj.CompanyID = (int)_session.GetInt32("Cid");
                obj.BillDate = DateTime.Now;
                _context.Bill.Add(obj);
                _context.SaveChanges();
                var DebtorGroupPhone = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(obj.DebtorGroupID)).Select(a=>a.DebtorGroupPhoneNumber).FirstOrDefault();
                SendSmsAsync("whatsapp:" + DebtorGroupPhone, obj.BilledTo, obj.BillAmount);
                return new JsonResult("Added Successfully!");
            }
            catch(DbUpdateException )
            {
                string exce= "Update 1";
                return new JsonResult(exce);
            }
        }

        public Task SendSmsAsync(string number,string debtorname, double message)
        {
            // Plug in your SMS service here to send a text message.
            // Your Account SID from twilio.com/console
            var accountSid = Options.WhatsappAccountIdentification;
            // Your Auth Token from twilio.com/console
            var authToken = Options.WhatsappAccountPassword;

            TwilioClient.Init(accountSid, authToken);

            return MessageResource.CreateAsync(
            to: new PhoneNumber(number),
            from: new PhoneNumber(Options.WhatsappAccountFrom),
             body: $"Hey! {debtorname}. Your bill has been generated for ₹{message}");
        }
    }
}