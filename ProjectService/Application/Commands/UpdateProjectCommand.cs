using Domain.Entities;
using MediatR;

namespace Application.Commands;

public abstract record UpdateProjectCommand(string Id, Project Project) : IRequest<bool>;