using Domain.Entities;
using MediatR;

namespace Application.Commands;

public record CreateProjectCommand(Project Project) : IRequest<Project>;