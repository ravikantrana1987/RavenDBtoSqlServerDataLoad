using System.Data;

namespace RavenDBtoSqlServerDataLoad.Abstractions
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
