using ApptManager.Models;
using System.Threading.Tasks;
namespace ApptManager.Repo.Services

{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequestDto mailRequest);
    }
}
