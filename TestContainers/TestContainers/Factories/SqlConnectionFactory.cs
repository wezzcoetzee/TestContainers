using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using TestContainers.Options;

namespace TestContainers.Factories;

public interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IOptions<ConnectionStringOptions> connectionStrings)
    {
        _connectionString = connectionStrings.Value.UserDb;
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}