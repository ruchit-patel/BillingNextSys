using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillingNextSys.Services;
using Easy_Http;
using Easy_Http.Builders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BillingNextSys.Pages.Message
{
    [Authorize]
    public class resendMessageModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;
        private readonly IWhatsappSender _smsSender;

        public resendMessageModel(BillingNextSys.Models.BillingNextSysContext context, IOptions<Whatsappoptions> optionsAccessor)
        {
            _context = context;
            Options = optionsAccessor.Value;
        }

        public Whatsappoptions Options { get; }
        public void OnGet()
        {
        }

        public IActionResult OnPostSendMessage([FromBody] Models.Bill obj)
        {


            var fyear = obj.Note;
         
            var DebtorGroupInfo = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(obj.DebtorGroupID)).FirstOrDefault();

            var format = "";

            if (string.IsNullOrEmpty(obj.SeriesName))
            {
                format = "Format2";
            }
            else
            {
                format = "Format1";
            }

            string companyname = _context.Company.Where(a => a.CompanyID.Equals(obj.CompanyID)).Select(ab => ab.CompanyName).FirstOrDefault().ToString();
            string[] msgcnt = new string[3];
            msgcnt[0]= SendSmsAsync(DebtorGroupInfo.DebtorGroupPhoneNumber.Substring(DebtorGroupInfo.DebtorGroupPhoneNumber.Length - 10), fyear, obj.BillAmount, format, Options.WhatsappAccountPassword, obj.BillNumber, obj.SecretUnlockCode, companyname, DebtorGroupInfo.DebtorOutstanding);
            msgcnt[1]=(DebtorGroupInfo.DebtorGroupPhoneNumber.Substring(DebtorGroupInfo.DebtorGroupPhoneNumber.Length - 10));
            msgcnt[2] = DebtorGroupInfo.DebtorGroupName;


            return new JsonResult(msgcnt);

        }

        public string SendSmsAsync(string number, string year, double billamt, string format, string hostname, string billnum, int secretcode, string compname, double debtorout)
        {
            var msgcnt = $"Professional bill for {year} is Rs. {billamt} and is due for payment.Kindly make payment. \n Thanks for doing business with {compname.Replace("&", "And")}. \nFind the bill here: {hostname}/Bill/{format}/Verify?id={billnum} \n Secret Code to unlock bill is: {secretcode} .";
             new RequestBuilder<string>()
    .SetHost($"http://api.msg91.com/api/sendhttp.php?route=4&sender={Options.WhatsappAccountFrom}&mobiles={number}&authkey={Options.WhatsappAccountIdentification}&message={msgcnt}&country=91")
    .SetContentType(ContentType.Application_Json)
    .SetType(RequestType.Get)
    .Build()
    .Execute();

            return msgcnt;
            // Plug in your SMS service here to send a text message.
            // Your Account SID from twilio.com/console
            //var accountSid = Options.WhatsappAccountIdentification;
            //// Your Auth Token from twilio.com/console
            //var authToken = Options.WhatsappAccountPassword;

            //TwilioClient.Init(accountSid, authToken);

            //return MessageResource.CreateAsync(
            //to: new PhoneNumber(number),
            //from: new PhoneNumber(Options.WhatsappAccountFrom),
            //body: $"Professional bill for quarter {quarter} of F.Y : {year} is ₹{billamt} and is due for payment. \n  \n Kindly make payment. \n Thanks for doing business with us. \nFind the bill here: {hostname}/Bill/{format}/Verify?id={billnum}   \n Secret Code to unlock bill is: {secretcode}");

        }
    }
    [Authorize]
    public class resendMessageGrid : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public resendMessageGrid(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public IQueryable<Models.Bill> Bills { get; set; }

        public void OnGet()
        {
            Bills = _context.Bill.Include(a => a.Company).Include(b => b.Branch).Where(a => a.MessageSent == true).AsQueryable();
        }
    }
}
