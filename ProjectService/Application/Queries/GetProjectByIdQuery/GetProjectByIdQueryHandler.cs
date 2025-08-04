using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Queries.GetProjectByIdQuery;

public class GetProjectByIdQueryHandler(IProjectRepository repository)
    : IRequestHandler<GetProjectByIdQuery, Project?>
{
    public async Task<Project?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(request.Id);
    }
}