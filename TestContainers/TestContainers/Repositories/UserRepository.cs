using Dapper;
using Microsoft.Data.SqlClient;
using TestContainers.Entities;
using TestContainers.Factories;

namespace TestContainers.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User> GetUserAsync(int id);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
}

public class UserRepository : IUserRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public UserRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<User>("SELECT Id, Name, Age FROM Users");
    }

    public async Task<User> GetUserAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<User>("SELECT Id, Name, Age FROM Users WHERE Id = @Id",
            new { Id = id });
    }

    public async Task AddUserAsync(User user)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync("INSERT INTO Users (Name, Age) VALUES (@Name, @Age)", user);
    }

    public async Task UpdateUserAsync(User user)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync("UPDATE Users SET Name = @Name, Age = @Age WHERE Id = @Id", user);
    }

    public async Task DeleteUserAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync("DELETE FROM Users WHERE Id = @Id", new { Id = id });
    }
}