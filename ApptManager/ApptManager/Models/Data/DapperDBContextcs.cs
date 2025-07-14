using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace ApptManager.Models.Data
{
    namespace WebApi.Models.Data
    {
        public class DapperDBContext: DbContext
        {
         
            private readonly IConfiguration _configuration;
            private readonly string connectionstring;
            public DapperDBContext(IConfiguration configuration)
            {
                this._configuration = configuration;
                this.connectionstring = this._configuration.GetConnectionString("DB");

            }
            public IDbConnection CreateConnection() => new SqlConnection(connectionstring);

        }

    }
}
