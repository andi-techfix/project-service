using Application.Commands;
using Application.Services;
using Domain.Repositories;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MongoDB.Driver;
using ProjectService.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mongoConfig = builder.Configuration.GetSection(MongoDbConfiguration.SectionName).Get<MongoDbConfiguration>();
MongoDbConfiguration.ThrowIfInvalid(mongoConfig);

var mongoConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING") ??
                            MongoDbConfiguration.GetConnectionString(mongoConfig!);
var mongoDatabaseName = Environment.GetEnvironmentVariable("MONGO_DATABASE_NAME") ?? mongoConfig!.DatabaseName;

builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnectionString));
builder.Services.AddSingleton(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongoDatabaseName);
});
builder.Services.AddSingleton(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return database.GetCollection<ProjectEntity>("projects");
});

builder.Services.AddSingleton<IProjectRepository, ProjectRepository>();
builder.Services.AddTransient<IMongoSeeder, MongoSeeder>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateProjectCommand>());

var userServiceUrl = Environment.GetEnvironmentVariable("USER_SERVICE_URL") ??
                     builder.Configuration.GetValue<string>("UserServiceUrl") ?? "http://localhost:5000";
builder.Services.AddHttpClient<IUserApiClient, UserApiClient>(client =>
{
    client.BaseAddress = new Uri(userServiceUrl);
});

var app = builder.Build();

// Seed MongoDB on startup
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IMongoSeeder>();
    seeder.SeedAsync().GetAwaiter().GetResult();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();