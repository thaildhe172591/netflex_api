using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Domain.Entities;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Actors.Queries;

public record GetActorQuery(long Id) : IQuery<ActorDto>;

public class GetActorQueryHandler(IActorReadOnlyRepository actorReadOnlyRepository) : IQueryHandler<GetActorQuery, ActorDto>
{
    public async Task<ActorDto> Handle(GetActorQuery request, CancellationToken cancellationToken)
    {
        var actor = await actorReadOnlyRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Actor), request.Id);
        return actor;
    }
}