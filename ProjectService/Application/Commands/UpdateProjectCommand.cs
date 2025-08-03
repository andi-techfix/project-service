using Domain.Entities;
using MediatR;

namespace Application.Commands;

public record UpdateProjectCommand(string Id, Project Project) : IRequest<bool>;