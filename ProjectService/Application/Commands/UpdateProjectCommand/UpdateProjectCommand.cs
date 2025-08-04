using Domain.Entities;
using MediatR;

namespace Application.Commands.UpdateProjectCommand;

public record UpdateProjectCommand(string Id, Project Project) : IRequest<bool>;