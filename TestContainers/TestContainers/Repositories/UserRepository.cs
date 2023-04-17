using Dapper;
using TestContainers.Entities;
using TestContainers.Factories;

namespace TestContainers.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> GetAsync(int id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
}

public class UserRepository : IUserRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public UserRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        await using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<User>("SELECT Id, Name, Age FROM Users");
    }

    public async Task<User> GetAsync(int id)
    {
        await using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<User>("SELECT Id, Name, Age FROM Users WHERE Id = @Id",
            new { Id = id });
    }

    public async Task AddAsync(User user)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync("INSERT INTO Users (Name, Age) VALUES (@Name, @Age)", user);
    }

    public async Task UpdateAsync(User user)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync("UPDATE Users SET Name = @Name, Age = @Age WHERE Id = @Id", user);
    }

    public async Task DeleteAsync(int id)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync("DELETE FROM Users WHERE Id = @Id", new { Id = id });
    }
}