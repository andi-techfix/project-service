using Domain.Entities;

namespace Domain.Repositories;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllAsync();
    
    Task<Project?> GetByIdAsync(string id);
    
    Task CreateAsync(Project project);
    
    Task UpdateAsync(string id, Project project);
    
    Task DeleteAsync(string id);
    
    Task<IEnumerable<Project>> GetProjectsByUserIdsAsync(IEnumerable<int> userIds);
}