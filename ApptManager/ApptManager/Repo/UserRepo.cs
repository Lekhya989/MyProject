using ApptManager.Models;
using ApptManager.Models.Data.WebApi.Models.Data;
using Dapper;
using System.Data;

namespace ApptManager.Repo
{
    public class UserRepo : GenericRepository<User>, IUserRepo
    {
        private readonly DapperDBContext _context;

        public UserRepo(DapperDBContext context) : base(context.CreateConnection())
        {
            _context = context;
        }

        public async Task<string> Create(User user)
        {
            const string query = @"
                INSERT INTO Users 
                  (FirstName, LastName, Email, Password, MobileNumber, CreatedOn, UserType)
                VALUES 
                  (@FirstName, @LastName, @Email, @Password, @MobileNumber, @CreatedOn, @UserType)";

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.CreatedOn = DateTime.UtcNow;
            user.UserType = user.UserType == UserType.NONE ? UserType.USER : user.UserType;

            var parameters = new DynamicParameters();
            parameters.Add("FirstName", user.FirstName);
            parameters.Add("LastName", user.LastName);
            parameters.Add("Email", user.Email);
            parameters.Add("Password", hashedPassword);
            parameters.Add("MobileNumber", user.MobileNumber);
            parameters.Add("CreatedOn", user.CreatedOn);
            parameters.Add("UserType", user.UserType.ToString());

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(query, parameters);

            return "Thank you for registering.";
        }

        public async Task<User?> GetByEmail(string email)
        {
            const string query = "SELECT * FROM Users WHERE Email = @Email";
            using var conn = _context.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
        }

        public async Task<string> Update(User user, int id)
        {
            user.Id = id;
            const string sql = @"
                UPDATE Users SET 
                  FirstName    = @FirstName,
                  LastName     = @LastName,
                  Email        = @Email,
                  MobileNumber = @MobileNumber,
                  UserType     = @UserType
                WHERE Id = @Id";

            await UpdateAsync(sql, user);
            return "User updated successfully.";
        }

    }
}
