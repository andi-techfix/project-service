using Domain.Repositories;
using MediatR;

namespace Application.Commands.UpdateProjectCommand;

public class UpdateProjectCommandHandler(IProjectRepository projectRepository)
    : IRequestHandler<UpdateProjectCommand, bool>
{
    public async Task<bool> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var existing = await projectRepository.GetByIdAsync(request.Id);
        if (existing == null) return false;
        request.Project.Id = request.Id;
        await projectRepository.UpdateAsync(request.Id, request.Project);
        return true;
    }
}