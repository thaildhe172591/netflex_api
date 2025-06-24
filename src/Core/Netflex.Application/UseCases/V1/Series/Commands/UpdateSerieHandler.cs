using FluentValidation;
using Netflex.Domain.Entities;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Series.Commands;

public record UpdateSerieCommand(
    long Id,
    string? Name,
    string? Overview,
    string? PosterPath,
    string? BackdropPath,
    string? CountryISO,
    DateTime? FirstAirDate,
    DateTime? LastAirDate,
    ICollection<long>? KeywordIds,
    ICollection<long>? GenreIds
) : ICommand<UpdateSerieResult>;

public record UpdateSerieResult(long Id);

public class UpdateSerieCommandValidator : AbstractValidator<UpdateSerieCommand>
{
    public UpdateSerieCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class UpdateSerieHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateSerieCommand, UpdateSerieResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UpdateSerieResult> Handle(UpdateSerieCommand request, CancellationToken cancellationToken)
    {
        var serieRepository = _unitOfWork.Repository<TVSerie>();

        var serie = serieRepository.Get(request.Id)
            ?? throw new NotFoundException(nameof(TVSerie), request.Id);

        serie.Update(
            request.Name,
            request.Overview,
            request.PosterPath,
            request.BackdropPath,
            request.FirstAirDate,
            request.LastAirDate,
            request.CountryISO
        );

        if (request.KeywordIds != null)
        {
            var keywords = request.KeywordIds.Count != 0
                ? await _unitOfWork.Repository<Keyword>()
                    .GetAllAsync(k => request.KeywordIds.Contains(k.Id), cancellationToken: cancellationToken) : [];
            serie.AssignKeywords(keywords);
        }

        if (request.GenreIds != null)
        {
            var genres = request.GenreIds.Count != 0
                ? await _unitOfWork.Repository<Genre>()
                    .GetAllAsync(g => request.GenreIds.Contains(g.Id), cancellationToken: cancellationToken) : [];
            serie.AssignGenres(genres);
        }

        serieRepository.Update(serie);
        await _unitOfWork.CommitAsync();
        return new UpdateSerieResult(serie.Id);
    }
}