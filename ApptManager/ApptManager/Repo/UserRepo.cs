using ApptManager.Models;
using ApptManager.Models.Data.WebApi.Models.Data;
using Dapper;
using System.Data;

namespace ApptManager.Repo
{
    public class UserRepo : IUserRepo
    {
        private readonly DapperDBContext context;
        public UserRepo(DapperDBContext context)
        {
            this.context = context;

        }

        public async Task<string> Create(UserObj user)
        {
            string response = string.Empty;
            string query = "Insert into Users(FirstName, LastName,Email,Password,MobileNumber,CreatedOn,UserType) values (@FirstName, @LastName,@Email,@Password,@MobileNumber,@CreatedOn,@UserType)";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.CreatedOn = DateTime.UtcNow;
            user.UserType = UserType.USER;
            if (user.UserType == UserType.NONE)
                user.UserType = UserType.USER;
            
            var parameters = new DynamicParameters();
            parameters.Add("Id", user.Id, DbType.Int32);
            parameters.Add("FirstName", user.FirstName, DbType.String);
            parameters.Add("LastName", user.LastName, DbType.String);
            parameters.Add("Email", user.Email, DbType.String);
            parameters.Add("Password", hashedPassword, DbType.String);
            parameters.Add("MobileNumber", user.MobileNumber, DbType.String);
            parameters.Add("CreatedOn", user.CreatedOn, DbType.DateTime);
            parameters.Add("UserType", user.UserType.ToString(), DbType.String);
            
            using (var connection = this.context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
                response = "Thank you for registering.";
            }
            Console.WriteLine(response);
            return response;
        }

        public async Task<List<UserObj>> GetAll()
        {
            string query = "Select * From Users";
            using (var connection = this.context.CreateConnection())
            {
                var userlist = await connection.QueryAsync<UserObj>(query);
                return userlist.ToList();
            }

        }

        public async Task<UserObj> GetbyId(int Id)
        {
            string query = "Select * From Users where Id = @Id";
            using (var connection = this.context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<UserObj>(query, new { Id });
                
            }
        }

        public async Task<string> Remove(int Id)
        {
            string response = string.Empty;
            string query = "Delete From Users where Id = @Id";
            using (var connection = this.context.CreateConnection())
            {
                var userlist = await connection.ExecuteAsync(query, new { Id });
                response = "pass";
            }
            return response;
        }

        public async Task<string> Update(UserObj user, int Id)
        {
            string response = string.Empty;
            string query = "Update Users set FirstName = @FirstName, LastName=@LastName, Email=@Email, MobileNumber=@MobileNumber, UserType=@UserType, AccountStatus=@AccountStatus where Id=@Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", Id, DbType.Int32);
            parameters.Add("FirstName", user.FirstName, DbType.String);
            parameters.Add("LastName", user.LastName, DbType.String);
            parameters.Add("Email", user.Email, DbType.String);
            parameters.Add("MobileNumber", user.MobileNumber, DbType.String);
            parameters.Add("UserType", user.UserType.ToString(), DbType.String);
          
            using (var connection = this.context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
                response = "pass";
            }
            return response;
        }

        public async Task<UserObj> GetByEmail(string email)
        {
            string query = "SELECT * FROM Users WHERE Email = @Email";
            using (var connection = this.context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<UserObj>(query, new { Email = email });
            }
        }


    }
}
