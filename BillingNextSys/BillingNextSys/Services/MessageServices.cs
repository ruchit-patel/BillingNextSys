using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BillingNextSys.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IWhatsappSender
    {
        public AuthMessageSender(IOptions<Whatsappoptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public Whatsappoptions Options { get; }  // set only via Secret Manager

       // public Task SendEmailAsync(string email, string subject, string message)
        //{
            // Plug in your email service here to send an email.
         //   return Task.FromResult(0);
        //}

        public Task SendSmsAsync(string number, string message)
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
              body: message);
        }
    }
}