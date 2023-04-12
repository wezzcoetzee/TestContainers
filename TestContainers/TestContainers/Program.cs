using TestContainers.Entities;
using TestContainers.Factories;
using TestContainers.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

var app = builder.Build();

app.MapGet("/api/users", async (IUserRepository userRepository) => await userRepository.GetUsersAsync());
app.MapGet("/api/users/{id}", async (IUserRepository userRepository, int id) => await userRepository.GetUserAsync(id));
app.MapPost("/api/users", async (IUserRepository userRepository, User user) => await userRepository.AddUserAsync(user));
app.MapPut("/api/users/{id}", async (IUserRepository userRepository, int id, User user) =>
{
    user.Id = id;
    await userRepository.UpdateUserAsync(user);
});
app.MapDelete("/api/users/{id}",
    async (IUserRepository userRepository, int id) => await userRepository.DeleteUserAsync(id));


app.Run();

// This is needed for integration tests
public partial class Program
{
}