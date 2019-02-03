using System.Threading.Tasks;

namespace BillingNextSys.Services
{
    public interface IWhatsappSender
    {
        Task SendSmsAsync(string number, string message);
    }
}