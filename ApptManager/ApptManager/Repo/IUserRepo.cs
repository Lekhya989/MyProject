using ApptManager.Models;

namespace ApptManager.Repo
{
    public interface IUserRepo
    {
        Task<List<UserObj>> GetAll();
        Task<UserObj>GetbyId(int Id);
        Task<string> Create(UserObj user);
        Task<string>Update(UserObj user, int Id);
        Task<string> Remove (int Id);
        Task<UserObj> GetByEmail(string Email); 

    }


}
