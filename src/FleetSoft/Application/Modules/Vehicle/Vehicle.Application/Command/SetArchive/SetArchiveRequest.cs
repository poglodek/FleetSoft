using MediatR;

namespace Vehicle.Application.Command.SetArchive;

public record SetArchiveRequest(Guid Id): IRequest<Unit>;