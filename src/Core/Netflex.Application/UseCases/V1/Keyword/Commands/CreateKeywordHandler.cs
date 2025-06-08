using FluentValidation;

namespace Netflex.Application.UseCases.V1.Keyword.Commands;

public record CreateKeywordCommand(string Name) : ICommand<CreateKeywordResult>;
public record CreateKeywordResult(long Id);

public class CreateKeywordCommandValidator : AbstractValidator<CreateKeywordCommand>
{
    public CreateKeywordCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreateKeywordHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<CreateKeywordCommand, CreateKeywordResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateKeywordResult> Handle(CreateKeywordCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Keyword>();

        var haveExisted = await repository.ExistsAsync(x => x.Name == request.Name, cancellationToken);
        if (haveExisted) throw new NameAlreadyExistsException(request.Name);

        var keyword = Domain.Entities.Keyword.Create(request.Name);
        await repository.AddAsync(keyword, cancellationToken);

        await _unitOfWork.CommitAsync();
        return new CreateKeywordResult(keyword.Id);
    }
}