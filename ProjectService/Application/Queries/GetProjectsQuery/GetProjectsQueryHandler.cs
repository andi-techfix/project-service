using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Queries.GetProjectsQuery;

public class GetProjectsQueryHandler(IProjectRepository repository)
    : IRequestHandler<GetProjectsQuery, IEnumerable<Project>>
{
    public async Task<IEnumerable<Project>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetAllAsync();
    }
}