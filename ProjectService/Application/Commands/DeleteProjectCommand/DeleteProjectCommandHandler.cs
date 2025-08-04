using Domain.Repositories;
using MediatR;

namespace Application.Commands.DeleteProjectCommand;

public class DeleteProjectCommandHandler(IProjectRepository projectRepository)
    : IRequestHandler<DeleteProjectCommand, bool>
{
    public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var existing = await projectRepository.GetByIdAsync(request.Id);
        if (existing == null) return false;
        await projectRepository.DeleteAsync(request.Id);
        return true;
    }
}