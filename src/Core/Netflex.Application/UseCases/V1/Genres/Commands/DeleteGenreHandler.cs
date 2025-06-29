using FluentValidation;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Genres.Commands;

public record DeleteGenreCommand(long Id) : ICommand;

public class DeleteGenreCommandValidator : AbstractValidator<DeleteGenreCommand>
{
    public DeleteGenreCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteGenreHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteGenreCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Genre>();

        var genre = repository.Get(request.Id)
            ?? throw new NotFoundException(nameof(Domain.Entities.Genre), request.Id);

        repository.Remove(genre);
        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}