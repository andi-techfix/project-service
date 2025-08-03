using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Models;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class ProjectRepository(IMongoCollection<ProjectEntity> projectsCollection) : IProjectRepository
{
    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        var entities = await projectsCollection.Find(_ => true).ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task<Project?> GetByIdAsync(string id)
    {
        var entity = await projectsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        return entity == null ? null : MapToDomain(entity);
    }

    public async Task CreateAsync(Project project)
    {
        var entity = MapToEntity(project);
        await projectsCollection.InsertOneAsync(entity);
        project.Id = entity.Id;
    }

    public async Task UpdateAsync(string id, Project project)
    {
        var entity = MapToEntity(project);
        entity.Id = id;
        await projectsCollection.ReplaceOneAsync(p => p.Id == id, entity);
    }

    public async Task DeleteAsync(string id)
    {
        await projectsCollection.DeleteOneAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Project>> GetProjectsByUserIdsAsync(IEnumerable<int> userIds)
    {
        if (!userIds.Any()) return [];
        var filter = Builders<ProjectEntity>.Filter.In(p => p.UserId, userIds);
        var entities = await projectsCollection.Find(filter).ToListAsync();
        return entities.Select(MapToDomain);
    }

    private static Project MapToDomain(ProjectEntity entity)
    {
        return new Project
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Name = entity.Name,
            Charts = entity.Charts.Select(c => new Chart
            {
                Symbol = c.Symbol,
                Timeframe = c.Timeframe,
                Indicators = c.Indicators.Select(i => new Indicator
                {
                    Name = i.Name,
                    Parameters = i.Parameters
                }).ToList()
            }).ToList()
        };
    }

    private static ProjectEntity MapToEntity(Project project)
    {
        return new ProjectEntity
        {
            Id = project.Id,
            UserId = project.UserId,
            Name = project.Name,
            Charts = project.Charts.Select(c => new ChartEntity
            {
                Symbol = c.Symbol,
                Timeframe = c.Timeframe,
                Indicators = c.Indicators.Select(i => new IndicatorEntity
                {
                    Name = i.Name,
                    Parameters = i.Parameters
                }).ToList()
            }).ToList()
        };
    }
}