using FluentValidation;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Users.Commands;

public record AssignRoleCommand(string UserId, string RoleName) : ICommand;

public class AssignRoleCommandValidator : AbstractValidator<AssignRoleCommand>
{
    public AssignRoleCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RoleName).NotEmpty();
    }
}

public class AssignRoleHandler(
    IUnitOfWork unitOfWork)
    : ICommandHandler<AssignRoleCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        var userRepository = _unitOfWork.Repository<Domain.Entities.User>();
        var roleRepository = _unitOfWork.Repository<Domain.Entities.Role>();

        var user = await userRepository
            .GetAsync(u => u.Id == request.UserId, includeProperties: ["Roles"], cancellationToken: cancellationToken)
            ?? throw new UserNotFoundException();

        var role = await roleRepository
            .GetAsync(r => r.Name == request.RoleName, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Role), request.RoleName);

        if (!user.Roles.Any(r => r.Name == role.Name))
            user.AssignRole([role]);

        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}