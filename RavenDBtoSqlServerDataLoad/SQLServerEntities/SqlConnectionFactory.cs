using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RavenDBtoSqlServerDataLoad.Abstractions;
using System.Data;

namespace RavenDBtoSqlServerDataLoad.SQLServerEntities
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SQLConnection") ??
                throw new ApplicationException("Connection String is missing."); 
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
