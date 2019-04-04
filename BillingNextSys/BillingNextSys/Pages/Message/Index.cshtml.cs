using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillingNextSys.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Rest;
using RestRequest;
using Easy_Http.Builders;
using Easy_Http;

namespace BillingNextSys.Pages.Message
{
    [Authorize]
    public class IndexModel : PageModel
    {
       
       
        private readonly BillingNextSys.Models.BillingNextSysContext _context;
        private readonly IWhatsappSender _smsSender;

        public IndexModel(BillingNextSys.Models.BillingNextSysContext context, IOptions<Whatsappoptions> optionsAccessor)
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

            //var mnth = obj.InvoiceDate.Month;
            //var quarter = "";
            //if(mnth==4 || mnth==5 || mnth==6)
            //{
            //    quarter = "1";
            //}
            //else if(mnth == 7 || mnth == 8 || mnth == 9)
            //{
            //    quarter = "2";
            //}
            //else if (mnth == 10 || mnth == 11 || mnth == 12)
            //{
            //    quarter = "3";
            //}
            //else
            //{
            //    quarter = "4";
            //}
            //if(quarter=="")
            //{
            //    return NotFound();
            //}
             var fyear = obj.Note;
            //if(quarter=="4")
            //{
            //    fyear ="" +((obj.InvoiceDate.Year)-1)+"-"+(obj.InvoiceDate.Year);
            //}
            //else
            //{ 
            //    fyear= "" + (obj.InvoiceDate.Year) + "-" + ((obj.InvoiceDate.Year)+1);
            //}

            var DebtorGroupInfo = _context.DebtorGroup.Where(a => a.DebtorGroupID.Equals(obj.DebtorGroupID)).FirstOrDefault();

            var format = "";

            if(string.IsNullOrEmpty(obj.SeriesName))
            {
                format = "Format2";
            }
            else
            {
                format = "Format1";
            }

            string companyname = _context.Company.Where(a => a.CompanyID.Equals(obj.CompanyID)).Select(ab => ab.CompanyName).FirstOrDefault().ToString();

             SendSmsAsync(DebtorGroupInfo.DebtorGroupPhoneNumber.Substring(DebtorGroupInfo.DebtorGroupPhoneNumber.Length-10), fyear, obj.BillAmount, format, Options.WhatsappAccountPassword, obj.BillNumber, obj.SecretUnlockCode,companyname, DebtorGroupInfo.DebtorOutstanding);

            var messagesent = new Models.Bill { BillNumber = obj.BillNumber, MessageSent = true };
            _context.Bill.Attach(messagesent).Property(x => x.MessageSent).IsModified = true;
            _context.SaveChanges();

            return new JsonResult("Sent Successfully!");

        }

        public async Task SendSmsAsync(string number, string year, double billamt, string format, string hostname,string billnum, int secretcode,string compname,double debtorout)
        {
            await new RequestBuilder<string>()
    .SetHost($"http://api.msg91.com/api/sendhttp.php?route=4&sender={Options.WhatsappAccountFrom}&mobiles={number}&authkey={Options.WhatsappAccountIdentification}&message=Professional bill for {year} is Rs. {billamt} and is due for payment. Kindly make payment. \n Thanks for doing business with {compname.Replace("&","And")}. \nFind the bill here: {hostname}/Bill/{format}/Verify?id={billnum}   \n Secret Code to unlock bill is: {secretcode} . Your total amount outstanding is Rs. {debtorout}. &country=91")
    .SetContentType(ContentType.Application_Json)
    .SetType(RequestType.Get)
    .Build()
    .Execute();
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
    public class IndexGridModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public IndexGridModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public IQueryable<Models.Bill> Bills { get; set; }

        public void OnGet()
        {
            Bills = _context.Bill.Where(a => a.MessageSent == false).AsQueryable();
        }
    }
}
