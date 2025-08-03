using Infrastructure.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Infrastructure.Services;

public interface IMongoSeeder
{
    Task SeedAsync();
}
    
public class MongoSeeder(
    IMongoCollection<ProjectEntity> projects,
    ILogger<MongoSeeder> logger)
    : IMongoSeeder
{
    public async Task SeedAsync()
    {
        var count = await projects.CountDocumentsAsync(FilterDefinition<ProjectEntity>.Empty);
        if (count > 0)
        {
            logger.LogInformation("MongoDB already seeded with {Count} projects", count);
            return;
        }
        logger.LogInformation("Seeding MongoDB projects collection");

        var sampleProjects = new List<ProjectEntity>
        {
            new()
            {
                UserId = 3,
                Name = "my super project 1",
                Charts = new List<ChartEntity>
                {
                    new()
                    {
                        Symbol = "EURUSD",
                        Timeframe = "M5",
                        Indicators = new List<IndicatorEntity>()
                    },
                    new()
                    {
                        Symbol = "USDJPY",
                        Timeframe = "H1",
                        Indicators = new List<IndicatorEntity>
                        {
                            new() { Name = "BB", Parameters = "a=1;b=2;c=3" },
                            new() { Name = "MA", Parameters = "a=1;b=2;c=3" }
                        }
                    }
                }
            },
            new()
            {
                UserId = 3,
                Name = "my super project 2",
                Charts = new List<ChartEntity>
                {
                    new()
                    {
                        Symbol = "EURUSD",
                        Timeframe = "M5",
                        Indicators = new List<IndicatorEntity>
                        {
                            new() { Name = "MA", Parameters = "a=1;b=2;c=3" }
                        }
                    }
                }
            },
            new()
            {
                UserId = 3,
                Name = "my super project 3",
                Charts = new List<ChartEntity>()
            },
            new()
            {
                UserId = 2,
                Name = "project 1",
                Charts = new List<ChartEntity>
                {
                    new()
                    {
                        Symbol = "EURUSD",
                        Timeframe = "H1",
                        Indicators = new List<IndicatorEntity>
                        {
                            new() { Name = "RSI", Parameters = "a=1;b=2;c=3" }
                        }
                    }
                }
            },
            new()
            {
                UserId = 2,
                Name = "project 2",
                Charts = new List<ChartEntity>
                {
                    new()
                    {
                        Symbol = "USDJPY",
                        Timeframe = "H1",
                        Indicators = new List<IndicatorEntity>
                        {
                            new() { Name = "MA", Parameters = "a=1;b=2;c=3" }
                        }
                    }
                }
            },
            new()
            {
                UserId = 1,
                Name = "project 3",
                Charts = new List<ChartEntity>
                {
                    new()
                    {
                        Symbol = "EURUSD",
                        Timeframe = "M5",
                        Indicators = new List<IndicatorEntity>
                        {
                            new() { Name = "RSI", Parameters = "a=1;b=2;c=3" },
                            new() { Name = "MA", Parameters = "a=1;b=2;c=3" }
                        }
                    }
                }
            }
        };

        await projects.InsertManyAsync(sampleProjects);
    }
}