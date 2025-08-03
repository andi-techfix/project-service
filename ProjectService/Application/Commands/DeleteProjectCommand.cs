using MediatR;

namespace Application.Commands
{
    /// <summary>
    /// Command for deleting a project by its identifier. Returns true on success.
    /// </summary>
    public record DeleteProjectCommand(string Id) : IRequest<bool>;
}