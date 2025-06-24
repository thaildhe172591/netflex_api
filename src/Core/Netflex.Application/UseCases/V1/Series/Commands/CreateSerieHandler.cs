using FluentValidation;
using Netflex.Domain.Entities;

namespace Netflex.Application.UseCases.V1.Series.Commands;

public record CreateSerieCommand(
    string Name,
    string? Overview,
    string? PosterPath,
    string? BackdropPath,
    string? CountryISO,
    DateTime? FirstAirDate,
    DateTime? LastAirDate,
    ICollection<long>? KeywordIds,
    ICollection<long>? GenreIds
) : ICommand<CreateSerieResult>;

public record CreateSerieResult(long Id);
public class CreateSerieCommandValidator : AbstractValidator<CreateSerieCommand>
{
    public CreateSerieCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreateSerieHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<CreateSerieCommand, CreateSerieResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateSerieResult> Handle(CreateSerieCommand request, CancellationToken cancellationToken)
    {
        var serie = TVSerie.Create(
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

        await _unitOfWork.Repository<TVSerie>().AddAsync(serie, cancellationToken: cancellationToken);
        await _unitOfWork.CommitAsync();
        return new CreateSerieResult(serie.Id);
    }
}