using ApptManager.Models;
using ApptManager.DTOs;

public interface IUserService
{
    Task<string> Create(CreateUserDto dto);
    Task<List<UserResponseDto>> GetAll();
    Task<UserResponseDto?> GetById(int id);
    Task<string> Update(UpdateUserDto dto, int id);
    Task<string> Remove(int id);
    Task<User?> GetByEmail(string email);
}
