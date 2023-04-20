using System.Threading.Tasks;

namespace CFAWebApi.MailSetting.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
