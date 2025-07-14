using ApptManager.Models;
using ApptManager.Repo;

namespace ApptManager.Repo.Services
{
    public interface IUserService
    {
        Task<List<UserObj>> GetAll();
        Task<UserObj> GetbyId(int Id);
        Task<string> Create(UserObj user);
        Task<string> Remove(int Id);
        Task<string> Update(UserObj user, int id);
        Task<UserObj> GetByEmail(string email);
    }
}
