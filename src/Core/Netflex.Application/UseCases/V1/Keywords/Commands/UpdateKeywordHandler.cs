using FluentValidation;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Keywords.Commands;

public record UpdateKeywordCommand(long Id, string Name) : ICommand;

public class UpdateKeywordCommandValidator : AbstractValidator<UpdateKeywordCommand>
{
    public UpdateKeywordCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UpdateKeywordHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateKeywordCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(UpdateKeywordCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Keyword>();

        var keyword = repository.Get(request.Id)
            ?? throw new NotFoundException(nameof(Domain.Entities.Keyword), request.Id);

        var haveExisted = await repository.ExistsAsync(x => x.Name == request.Name && x.Id != request.Id, cancellationToken);
        if (haveExisted) throw new NameAlreadyExistsException(request.Name);

        keyword.Update(request.Name);
        repository.Update(keyword);
        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}