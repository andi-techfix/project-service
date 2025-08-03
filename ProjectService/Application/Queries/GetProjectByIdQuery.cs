using Domain.Entities;
using MediatR;

namespace Application.Queries;

public record GetProjectByIdQuery(string Id) : IRequest<Project?>;