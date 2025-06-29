using FluentValidation;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Genres.Commands;

public record UpdateGenreCommand(long Id, string Name) : ICommand;

public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
{
    public UpdateGenreCommandValidator()
    {
        RuleFor(x => x.Id).NotNull();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UpdateGenreHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateGenreCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Genre>();

        var genre = repository.Get(request.Id)
            ?? throw new NotFoundException(nameof(Domain.Entities.Genre), request.Id);

        var haveExisted = await repository.ExistsAsync(x => x.Name == request.Name && x.Id != request.Id, cancellationToken);
        if (haveExisted) throw new NameAlreadyExistsException(request.Name);

        genre.Update(request.Name);
        repository.Update(genre);
        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}