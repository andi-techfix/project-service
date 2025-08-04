using Domain.Entities;
using MediatR;

namespace Application.Commands.CreateProjectCommand;

public record CreateProjectCommand(Project Project) : IRequest<Project>;