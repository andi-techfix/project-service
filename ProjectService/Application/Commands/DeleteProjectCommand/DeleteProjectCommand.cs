using MediatR;

namespace Application.Commands.DeleteProjectCommand;

public record DeleteProjectCommand(string Id) : IRequest<bool>;