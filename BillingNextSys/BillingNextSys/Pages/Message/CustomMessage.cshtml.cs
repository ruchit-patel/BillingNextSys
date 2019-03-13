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
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BillingNextSys.Pages.Message
{
    [Authorize]
    public class CustomMessageModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;
        private readonly IWhatsappSender _smsSender;

        public CustomMessageModel(BillingNextSys.Models.BillingNextSysContext context, IOptions<Whatsappoptions> optionsAccessor)
        {
            _context = context;
            Options = optionsAccessor.Value;
        }

        public Whatsappoptions Options { get; }

        public void OnGet()
        {

        }


        public IActionResult OnPostSendMessage([FromBody] Models.DebtorGroup obj)
        {
        
            SendSmsAsync( obj.DebtorGroupPhoneNumber.Substring(obj.DebtorGroupPhoneNumber.Length-10),obj.DebtorGroupCity);
                return new JsonResult("Sent Successfully!");
        }
        public async Task SendSmsAsync(string number, string msg)
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
            //body: $"{msg}");

            await new RequestBuilder<string>()
   .SetHost($"http://api.msg91.com/api/sendhttp.php?route=4&sender={Options.WhatsappAccountFrom}&mobiles={number}&authkey={Options.WhatsappAccountIdentification}&message={msg}&country=91")
   .SetContentType(ContentType.Application_Json)
   .SetType(RequestType.Get)
   .Build()
   .Execute();

        }
    }
    [Authorize]
    public class DebtorPartialModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public DebtorPartialModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public IQueryable<Models.DebtorGroup> DebtorGroups { get; set; }

        public void OnGet()
        {
            DebtorGroups = _context.DebtorGroup.AsQueryable();
        }
    }
}
