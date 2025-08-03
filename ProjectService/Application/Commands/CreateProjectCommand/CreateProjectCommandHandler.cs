using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Commands.CreateProjectCommand;

public class CreateProjectCommandHandler(IProjectRepository projectRepository)
    : IRequestHandler<CreateProjectCommand, Project>
{
    public async Task<Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        await projectRepository.CreateAsync(request.Project);
        return request.Project;
    }
}