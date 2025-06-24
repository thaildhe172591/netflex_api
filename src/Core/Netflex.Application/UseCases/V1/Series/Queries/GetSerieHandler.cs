using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Domain.Entities;
using Netflex.Shared.Exceptions;
namespace Netflex.Application.UseCases.V1.Series.Queries;

public record GetSerieQuery(long Id)
    : IQuery<SerieDetailDto>;

public class GetSerieHandler(ISerieReadOnlyRepository serieReadOnlyRepository)
    : IQueryHandler<GetSerieQuery, SerieDetailDto>
{
    private readonly ISerieReadOnlyRepository _serieReadOnlyRepository = serieReadOnlyRepository;
    public async Task<SerieDetailDto> Handle(GetSerieQuery request, CancellationToken cancellationToken)
    {
        var result = await _serieReadOnlyRepository.GetSerieAsync(
            request.Id,
            cancellationToken
        ) ?? throw new NotFoundException(nameof(TVSerie), request.Id);
        return result;
    }
}