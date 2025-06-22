using Netflex.Domain.ValueObjects;
using FluentValidation;

namespace Netflex.Application.UseCases.V1.Users.Commands;

public record CreateUserResult(string Id);
public record CreateUserCommand(string Email, string Password) : ICommand<CreateUserResult>;

public class CreateUserCommandValidator
    : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email must be a valid email address");
        RuleFor(x => x.Password)
            .Length(6, 20)
            .WithMessage("Password must be between 6 and 20 characters");
    }
}

public class CreateUserHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<CreateUserCommand, CreateUserResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<CreateUserResult> Handle(CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var userRepository = _unitOfWork.Repository<Domain.Entities.User>();
        var userExists = await userRepository.ExistsAsync(u => u.Email == Email.Of(request.Email), cancellationToken);

        if (userExists) throw new EmailAlreadyExistsException();

        try
        {
            var user = Domain.Entities.User.Create
            (
                Guid.NewGuid().ToString(),
                Email.Of(request.Email),
                HashString.Of(request.Password)
            );

            await userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync();

            return new CreateUserResult(user.Id);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}