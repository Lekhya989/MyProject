using ApptManager.Models;
using ApptManager.Models.Data.WebApi.Models.Data;
using Dapper;
using System.Data;

namespace ApptManager.Repo
{
    public class TaxProfessionalRepo : GenericRepository<TaxProfessional>, ITaxProfessionalRepo
    {
        private readonly DapperDBContext context;

        public TaxProfessionalRepo(DapperDBContext context) : base(context.CreateConnection())
        {
            this.context = context;
        }

    // ... rest of the code



        public async Task<string> Create(TaxProfessional taxPro)
        {
            string query = @"
                INSERT INTO TaxProfessionals 
                (FirstName, LastName, Email, PhoneNumber, speaks_spanish, SMB_certified, CreatedOn)
                VALUES 
                (@FirstName, @LastName, @Email, @PhoneNumber, @speaks_spanish, @SMB_certified, @CreatedOn)";

            taxPro.CreatedOn = DateTime.UtcNow;

            var parameters = new DynamicParameters();
            parameters.Add("FirstName", taxPro.FirstName, DbType.String);
            parameters.Add("LastName", taxPro.LastName, DbType.String);
            parameters.Add("Email", taxPro.Email, DbType.String);
            parameters.Add("PhoneNumber", taxPro.PhoneNumber, DbType.String);
            parameters.Add("speaks_spanish", taxPro.speaks_spanish, DbType.Boolean);
            parameters.Add("SMB_certified", taxPro.SMB_certified, DbType.Boolean);
            parameters.Add("CreatedOn", taxPro.CreatedOn, DbType.DateTime);

            using var connection = context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);

            return "Tax Professional added successfully.";
        }

        public async Task<string> Update(TaxProfessional taxPro, int id)
        {
            string query = @"
                UPDATE TaxProfessionals 
                SET 
                    FirstName = @FirstName,
                    LastName = @LastName,
                    Email = @Email,
                    PhoneNumber = @PhoneNumber,
                    speaks_spanish = @speaks_spanish,
                    SMB_certified = @SMB_certified,
                    UpdatedOn = @UpdatedOn
                WHERE Id = @Id";

            taxPro.UpdatedOn = DateTime.UtcNow;

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("FirstName", taxPro.FirstName, DbType.String);
            parameters.Add("LastName", taxPro.LastName, DbType.String);
            parameters.Add("Email", taxPro.Email, DbType.String);
            parameters.Add("PhoneNumber", taxPro.PhoneNumber, DbType.String);
            parameters.Add("speaks_spanish", taxPro.speaks_spanish, DbType.Boolean);
            parameters.Add("SMB_certified", taxPro.SMB_certified, DbType.Boolean);
            parameters.Add("UpdatedOn", taxPro.UpdatedOn, DbType.DateTime);

            using var connection = context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);

            return "Tax Professional updated successfully.";
        }

        public async Task<string> Remove(int id)
        {
            string query = "DELETE FROM TaxProfessionals WHERE Id = @Id";

            using var connection = context.CreateConnection();
            await connection.ExecuteAsync(query, new { Id = id });

            return "Tax Professional removed successfully.";
        }
    }
}
