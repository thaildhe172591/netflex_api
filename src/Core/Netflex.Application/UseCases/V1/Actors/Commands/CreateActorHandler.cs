using FluentValidation;
using Netflex.Application.Extensions;
using Netflex.Domain.Entities;
namespace Netflex.Application.UseCases.V1.Actors.Commands;

public record CreateActorCommand(
    string Name,
    IFileResource? Image,
    bool Gender,
    DateTime? BirthDate,
    string? Biography
) : ICommand<CreateActorResult>;
public record CreateActorResult(long Id);

public class CreateActorCommandValidator : AbstractValidator<CreateActorCommand>
{
    public CreateActorCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Image).MaxFileSize(5)
            .AllowedExtensions(".jpg", ".jpeg", ".png", ".webp");
    }
}

public class CreateActorHandler(IUnitOfWork unitOfWork, ICloudStorage storage)
    : ICommandHandler<CreateActorCommand, CreateActorResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICloudStorage _storage = storage;

    public async Task<CreateActorResult> Handle(CreateActorCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Actor>();

        var image = request.Image != null
            ? await _storage.UploadAsync("actor", request.Image) : null;

        var actor = Actor.Create(request.Name, request.Gender, request.BirthDate, request.Biography, image?.ToString());
        await repository.AddAsync(actor, cancellationToken);

        await _unitOfWork.CommitAsync();
        return new CreateActorResult(actor.Id);
    }
}