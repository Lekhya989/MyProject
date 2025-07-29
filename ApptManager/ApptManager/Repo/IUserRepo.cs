using ApptManager.Models;

namespace ApptManager.Repo
{
    public interface IUserRepo : IGenericRepository<User>
    {
        Task<string> Create(User user); // Custom create with password hashing
        Task<User?> GetByEmail(string email); // Custom email lookup
        Task<string> Update(User user, int id);        // Custom update
      

    }
}
