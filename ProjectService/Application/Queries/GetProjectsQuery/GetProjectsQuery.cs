using Domain.Entities;
using MediatR;

namespace Application.Queries.GetProjectsQuery;

public record GetProjectsQuery : IRequest<IEnumerable<Project>>;