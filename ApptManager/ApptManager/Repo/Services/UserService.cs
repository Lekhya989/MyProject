using ApptManager.DTOs;
using ApptManager.Models;
using ApptManager.Repo;
using ApptManager.Repo.Services;
using ApptManager.UnitOfWork;
using AutoMapper;

namespace ApptManager.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMailService mailService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mailService = mailService;
            _mapper = mapper;
        }

        public async Task<string> Create(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);

            var result = await _unitOfWork.Users.Create(user);

            if (result == "Thank you for registering.")
            {
                try
                {
                    var mailRequest = new MailRequestDto
                    {
                        ToEmail = user.Email,
                        Subject = "Registration Successful - Tax Pros",
                        Body = $"Hello {user.FirstName},<br/><br/>" +
                               $"Thank you for registering with <b>Tax Pros</b>!<br/><br/>" +
                               $"Regards,<br/>Tax Pros Team"
                    };
                    await _mailService.SendEmailAsync(mailRequest);
                }
                catch
                {
                    // Swallow email errors
                }
            }

            return result;
        }

        public async Task<List<UserResponseDto>> GetAll()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return _mapper.Map<List<UserResponseDto>>(users);
        }

        public async Task<UserResponseDto?> GetById(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserResponseDto>(user);
        }

        public async Task<string> Update(UpdateUserDto dto, int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                return "User not found.";

            _mapper.Map(dto, user); // Apply updated fields
            return await _unitOfWork.Users.Update(user, id);
        }

        public async Task<string> Remove(int id)
        {
            var rows = await _unitOfWork.Users.DeleteAsync(id);
            return rows > 0
                ? "User deleted successfully."
                : "No user found to delete.";
        }

        public async Task<User?> GetByEmail(string email)
            => await _unitOfWork.Users.GetByEmail(email); // Used for login only (entity needed)
    }
}
