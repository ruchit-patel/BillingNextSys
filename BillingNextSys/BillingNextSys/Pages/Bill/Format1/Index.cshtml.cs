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

namespace BillingNextSys.Pages.Bill.Format1
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

        public async Task<IActionResult> OnGetSelectAllAsync(string id)
        {
            List<Models.BillDetails> data = await _context.BillDetails.Where(ab => ab.BillNumber.Equals(id)).ToListAsync();
            return new JsonResult(data);
        }

        public IActionResult OnPostInsertReceived(int dgid, [FromBody] Models.Received obj)
        {
            obj.CompanyID = (int)_session.GetInt32("Cid");
            obj.DebtorGroupID = _context.BillDetails.Where(a => a.BillDetailsID.Equals(obj.BillDetailsID)).Select(a => a.DebtorGroupID).FirstOrDefault();
            _context.Received.Add(obj);
            _context.SaveChanges();

            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var amtout = _context.BillDetails.Where(a => a.BillDetailsID.Equals(obj.BillDetailsID)).FirstOrDefault().BillAmountOutstanding;
            // var amtout = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(dgid)).FirstOrDefault().DebtorOutstanding;
            var billout = amtout - obj.ReceivedAmount;
            var billdet = new Models.BillDetails { BillDetailsID = obj.BillDetailsID, BillAmountOutstanding = billout };
            _context.BillDetails.Attach(billdet).Property(x => x.BillAmountOutstanding).IsModified = true;
            _context.SaveChanges();

            var dgout = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(dgid)).FirstOrDefault().DebtorOutstanding;

            var debout = dgout - obj.ReceivedAmount;
            var dgdet = new Models.DebtorGroup { DebtorGroupID = dgid, DebtorOutstanding = debout };
            _context.DebtorGroup.Attach(dgdet).Property(x => x.DebtorOutstanding).IsModified = true;
            _context.SaveChanges();

            return new JsonResult("Successful!");
        }

        public IActionResult OnPost()
        {
            string amount = Request.Form["recamt"].ToString();
            string chequepayment = Request.Form["chqpaymt"].ToString();
            string receivedate = Request.Form["redate"].ToString();


           var paymode = "";
            if (string.IsNullOrEmpty(chequepayment))
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
            SendSmsAsync(DebtorGPhone.Substring(DebtorGPhone.Length - 10), DebtorName, amount, paymode, receivedate, CompanyName);

            return Page();
        }

        public async Task SendSmsAsync(string number, string name, string amount, string mode, string recdate, string CompanyName)
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
            var msg = $"Dear {name}, Thank you for the payment of Rs.{amount} by {mode} towards Receipt on {recdate}. \nThank you for showing interest in {CompanyName.Replace("&", "And")}";
            await new RequestBuilder<string>()
            .SetHost($"http://api.msg91.com/api/sendhttp.php?route=4&sender={Options.WhatsappAccountFrom}&mobiles={number}&authkey={Options.WhatsappAccountIdentification}&message={msg}&country=91")
            .SetContentType(ContentType.Application_Json)
            .SetType(RequestType.Get)
            .Build()
            .Execute();

        }

        public IActionResult OnPostAmtAdvance( [FromBody] Models.AdvancePay obj)
        {
            try
            {
                obj.CompanyID= (int)_session.GetInt32("Cid");
                obj.BranchID = (int)_session.GetInt32("Bid");
                _context.AdvancePay.Add(obj);
                _context.SaveChanges();
                return new JsonResult(1);
            }
            catch(Exception e)
            {
                return new JsonResult(0);
            }
            
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
                Bills = _context.Bill.Where(a => a.BillActNum.HasValue).Where(a=>a.CompanyID.Equals(cid)).Where(a=>a.BranchID.Equals(bid)).Include(b => b.Company).Include(b => b.DebtorGroup).AsQueryable();
                return Page();
            }
            catch (InvalidOperationException)
            {
                return RedirectToPage("/Index");
            }
        }
        }

    }