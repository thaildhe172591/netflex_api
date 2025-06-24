using FluentValidation;
using Netflex.Domain.Entities;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Movies.Commands;

public record DeleteMovieCommand(long Id) : ICommand;
public class DeleteMovieCommandValidator : AbstractValidator<DeleteMovieCommand>
{
    public DeleteMovieCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteMovieHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteMovieCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var movieRepository = _unitOfWork.Repository<Movie>();

        var movie = await movieRepository
            .GetAsync(m => m.Id == request.Id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Movie), request.Id);

        movieRepository.Remove(movie);
        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}