using System.Net.Http.Headers;
using Application.Services;
using Domain.Repositories;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using MongoDB.Driver;
using Polly;

namespace Infrastructure;

public static class ServiceInjection
{
    public static void AddMongoDbServices(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(MongoDbConfiguration.SectionName);
        services.Configure<MongoDbConfiguration>(section);
        var mongoConfig = section.Get<MongoDbConfiguration>();
        MongoDbConfiguration.ThrowIfInvalid(mongoConfig);
        
        var connString = configuration.GetValue<string>("MONGO_CONNECTION_STRING")
                         ?? MongoDbConfiguration.GetConnectionString(mongoConfig!);
        var dbName = configuration.GetValue<string>("MONGO_DATABASE_NAME")
                     ?? mongoConfig!.DatabaseName;
        
        services.AddSingleton<IMongoClient>(_ => new MongoClient(connString));
        services.AddSingleton(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(dbName);
        });
        
        services.AddSingleton(sp =>
        {
            var db = sp.GetRequiredService<IMongoDatabase>();
            return db.GetCollection<ProjectEntity>("projects");
        });
        services.AddSingleton<IProjectRepository, ProjectRepository>();
        services.AddTransient<IMongoSeeder, MongoSeeder>();
    }

    public static void AddUserApiClient(this IServiceCollection services, IConfiguration configuration)
    {
        var userServiceUrl = configuration["USER_SERVICE_URL"]
                             ?? throw new ArgumentNullException(nameof(configuration),
                                 "USER_SERVICE_URL is not configured");

        services.AddHttpClient<IUserApiClient, UserApiClient>((_, client) =>
            {
                client.BaseAddress = new Uri(userServiceUrl);
                client.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            })
            .AddApiResilienceHandler("UserServiceResilience");
    }

    private static void AddApiResilienceHandler(this IHttpClientBuilder builder, string name)
    {
        builder.AddResilienceHandler(name, resilienceBuilder =>
        {
            resilienceBuilder.AddTimeout(TimeSpan.FromSeconds(8));

            resilienceBuilder.AddRetry(new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(2),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true
            });

            resilienceBuilder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
            {
                SamplingDuration = TimeSpan.FromSeconds(10),
                FailureRatio = 0.1,
                MinimumThroughput = 3,
                BreakDuration = TimeSpan.FromSeconds(2)
            });

            resilienceBuilder.AddTimeout(TimeSpan.FromSeconds(4));
        });
    }
}