using FluentValidation;
using Netflex.Application.Extensions;
using Netflex.Domain.Entities;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Actors.Commands;

public record UpdateActorCommand(
    long Id,
    string? Name,
    IFileResource? Image,
    bool? Gender,
    DateTime? BirthDate,
    string? Biography
) : ICommand;

public class UpdateActorCommandValidator : AbstractValidator<UpdateActorCommand>
{
    public UpdateActorCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Image).MaxFileSize(5)
           .AllowedExtensions(".jpg", ".jpeg", ".png", ".webp");
    }
}

public class UpdateActorHandler(IUnitOfWork unitOfWork, ICloudStorage storage)
    : ICommandHandler<UpdateActorCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICloudStorage _storage = storage;

    public async Task<Unit> Handle(UpdateActorCommand request, CancellationToken cancellationToken)
    {
        var image = request.Image != null
               ? await _storage.UploadAsync("actor", request.Image) : null;
        var repository = _unitOfWork.Repository<Actor>();
        var actor = repository.Get(request.Id)
            ?? throw new NotFoundException(nameof(Actor), request.Id);

        actor.Update(
            request.Name,
            image?.ToString(),
            request.Gender,
            request.BirthDate,
            request.Biography
        );
        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}