using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillingNextSys.Services;
using Easy_Http;
using Easy_Http.Builders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BillingNextSys.Pages.Bill.Format2
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IWhatsappSender _smsSender;

        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public IndexModel(BillingNextSys.Models.BillingNextSysContext context, IHttpContextAccessor httpContextAccessor, IOptions<Whatsappoptions> optionsAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            Options = optionsAccessor.Value;
        }

        public Whatsappoptions Options { get; }
        public void OnGet()
        {
        }

        public IActionResult OnGetSelectAll(string id)
        {
            var query = from billdetails in _context.BillDetails
                        join debtor in _context.Debtor on billdetails.DebtorID equals debtor.DebtorID into gj
                        from subset in gj.DefaultIfEmpty()
                        where billdetails.BillNumber.Equals(id)
                        select new { billdetails, DebtorName = subset.DebtorName ?? String.Empty };


            return new JsonResult(query);
        }

        public IActionResult OnPost()
        {
            string amount = Request.Form["recamt"].ToString();
            string chequepayment = Request.Form["chqpaymt"].ToString();
            string receivedate = Request.Form["redate"].ToString();
      

            var paymode = "";
            if(string.IsNullOrEmpty(chequepayment))
            {
                paymode = "Cash";
            }
            else
            {
                paymode = "Cheque";
            }
            var DebtorGPhone = Request.Form["dphone"].ToString();
            var DebtorName = Request.Form["dgname"].ToString();
            var CompanyName = Request.Form["compname"].ToString();
            SendSmsAsync(DebtorGPhone.Substring(DebtorGPhone.Length - 10) ,DebtorName,amount,paymode,receivedate,CompanyName);

            return Page();
        }

        public async Task SendSmsAsync(string number,string name,string amount, string mode, string recdate, string CompanyName)
        {
            // Plug in your SMS service here to send a text message.
            // Your Account SID from twilio.com/console
            //var accountSid = Options.WhatsappAccountIdentification;
            //// Your Auth Token from twilio.com/console
            //var authToken = Options.WhatsappAccountPassword;

            //TwilioClient.Init(accountSid, authToken);

            //return MessageResource.CreateAsync(
            //to: new PhoneNumber(number),
            //from: new PhoneNumber(Options.WhatsappAccountFrom),
            var msg= $"Dear {name}, Thank you for the payment of Rs.{amount} by {mode} towards Receipt on {recdate}. \nThank you for showing interest in {CompanyName.Replace("&", "And")}";
            await new RequestBuilder<string>()
 .SetHost($"http://api.msg91.com/api/sendhttp.php?route=4&sender={Options.WhatsappAccountFrom}&mobiles={number}&authkey={Options.WhatsappAccountIdentification}&message={msg}&country=91")
 .SetContentType(ContentType.Application_Json)
 .SetType(RequestType.Get)
 .Build()
 .Execute();
        }
    }
    [Authorize]
    public class IndexGridModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public IndexGridModel(BillingNextSys.Models.BillingNextSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IQueryable<Models.Bill> Bills { get; set; }

        public IActionResult OnGet()
        {
            try
            {
                int cid = (int)_session.GetInt32("Cid");
                int bid = (int)_session.GetInt32("Bid");
                Bills = _context.Bill.Where(ab => !(ab.BillActNum.HasValue)).Where(a => a.CompanyID.Equals(cid)).Where(a => a.BranchID.Equals(bid)).Include(b => b.Company).Include(b => b.DebtorGroup).AsQueryable();
                return Page();
            }
            catch (InvalidOperationException)
            {
                return RedirectToPage("/Index");
            }
        }
    }
}
