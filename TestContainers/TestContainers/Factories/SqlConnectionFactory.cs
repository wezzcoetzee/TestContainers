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
    private readonly SqlConnection _sqlConnection;

    public SqlConnectionFactory(IOptions<ConnectionStringOptions> connectionStrings)
    {
        _sqlConnection = new SqlConnection(connectionStrings.Value.UserDb);
    }

    public SqlConnection CreateConnection()
    {
        return _sqlConnection;
    }
}