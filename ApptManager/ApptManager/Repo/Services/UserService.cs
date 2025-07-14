using ApptManager.Models;
using ApptManager.Repo;
using ApptManager.Repo.Services;

namespace ApptManager.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMailService _mailService;

        public UserService(IUserRepo userRepo, IMailService mailService)
        {
            _userRepo = userRepo;
            _mailService = mailService;
        }

        public async Task<string> Create(UserObj user)
        {
            var result = await _userRepo.Create(user);

            if (result == "Thank you for registering.")
            {
                Console.WriteLine("I am here");
                try
                {
                    var subject = "Registration Successful - Tax Pros";
                    var body = $"Hello {user.FirstName},<br/><br/>" +
                               $"Thank you for registering with <b>Tax Pros</b>!<br/><br/>" +
                               $"Regards,<br/>Tax Pros Team";

                    var mailRequest = new MailRequest
                    {
                        ToEmail = user.Email,
                        Subject = subject,
                        Body = body
                    };

                    await _mailService.SendEmailAsync(mailRequest);

                    Console.WriteLine($"Email sent to {user.Email}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send registration email: {ex.Message}");
                    // Email sending failed — continue without failing the registration
                }
            }

            return result;
        }

        public Task<List<UserObj>> GetAll()
        {
            return _userRepo.GetAll();
        }

        public Task<UserObj> GetbyId(int Id)
        {
            return _userRepo.GetbyId(Id);
        }

        public Task<string> Remove(int Id)
        {
            return _userRepo.Remove(Id);
        }

        public Task<string> Update(UserObj user, int Id)
        {
            return _userRepo.Update(user, Id);
        }

        public Task<UserObj> GetByEmail(string email)
        {
            return _userRepo.GetByEmail(email);
        }
    }
}