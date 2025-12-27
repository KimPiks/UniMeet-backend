using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.Users;
using UniMeet.UserModule.Domain.Users.Exceptions;

namespace UniMeet.UserModule.Application.Users.SetGroup;

public class SetGroupCommandHandler(IUserRepository repository) : ICommandHandler<SetGroupCommand>
{
    public async Task HandleAsync(SetGroupCommand request, CancellationToken cancellationToken = default)
    {
        var user = await repository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
            throw new UserWithIdNotFoundException(request.UserId);

        user.SetGroup(request.GroupId);
        await repository.SaveChangesAsync(cancellationToken);
    }
}