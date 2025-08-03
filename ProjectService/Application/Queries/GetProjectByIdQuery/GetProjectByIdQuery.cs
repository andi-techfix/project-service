using Domain.Entities;
using MediatR;

namespace Application.Queries.GetProjectByIdQuery;

public record GetProjectByIdQuery(string Id) : IRequest<Project?>;