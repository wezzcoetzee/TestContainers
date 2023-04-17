using TestContainers.Entities;
using TestContainers.Factories;
using TestContainers.Options;
using TestContainers.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var configuration = builder.Configuration;
builder.Services
    .Configure<ConnectionStringOptions>(configuration.GetSection(nameof(ConnectionStringOptions)));

builder.Services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

app.MapGet("/api/users", async (IUserRepository userRepository) => await userRepository.GetAllAsync());
app.MapGet("/api/users/{id}", async (IUserRepository userRepository, int id) => await userRepository.GetAsync(id));
app.MapPost("/api/users", async (IUserRepository userRepository, User user) => await userRepository.AddAsync(user));
app.MapPut("/api/users/{id}", async (IUserRepository userRepository, int id, User user) =>
{
    user.Id = id;
    await userRepository.UpdateAsync(user);
});
app.MapDelete("/api/users/{id}",
    async (IUserRepository userRepository, int id) => await userRepository.DeleteAsync(id));


app.Run();

// This is needed for integration tests
public partial class Program
{
}