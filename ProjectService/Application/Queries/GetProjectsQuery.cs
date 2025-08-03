using Domain.Entities;
using MediatR;

namespace Application.Queries;

public record GetProjectsQuery : IRequest<IEnumerable<Project>>;