using FluentValidation;

namespace Netflex.Application.UseCases.V1.Genres.Commands;

public record CreateGenreCommand(string Name) : ICommand<CreateGenreResult>;
public record CreateGenreResult(long Id);

public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
{
    public CreateGenreCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreateGenreHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<CreateGenreCommand, CreateGenreResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateGenreResult> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Genre>();

        var haveExisted = await repository.ExistsAsync(x => x.Name == request.Name, cancellationToken);
        if (haveExisted) throw new NameAlreadyExistsException(request.Name);

        var genre = Domain.Entities.Genre.Create(request.Name);
        await repository.AddAsync(genre, cancellationToken);

        await _unitOfWork.CommitAsync();
        return new CreateGenreResult(genre.Id);
    }
}