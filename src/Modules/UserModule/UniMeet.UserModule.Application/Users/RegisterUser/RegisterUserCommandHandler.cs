using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Application.Universities.GetByAllowedDomain;
using UniMeet.UserModule.Domain.Services;
using UniMeet.UserModule.Domain.Users;
using UniMeet.UserModule.Domain.Users.Exceptions;

namespace UniMeet.UserModule.Application.Users.RegisterUser;

public class RegisterUserCommandHandler(IUserRepository userRepository, 
    IPasswordHasher passwordHasher,
    IMediator mediator)
    : ICommandHandler<RegisterUserCommand> 
{
    public async Task HandleAsync(RegisterUserCommand request, CancellationToken cancellationToken = default)
    {
        request.Validate();
        
        // TODO: CHECKING IF USER ALREADY EXISTS (AND OTHER THINGS IN OTHER MODULES)

        // Check if a user has a university email
        var emailDomain = request.Email.Split("@")[1];
        var university = await mediator.SendAsync(new GetByAllowedDomainQuery(emailDomain), cancellationToken);
        if (university == null)
            throw new EmailDomainNotAllowedException(request.Email);
        
        var passwordHash = passwordHasher.Hash(request.Password);
        var user = new User(request.FirstName, request.LastName, request.Email, passwordHash, university.Id);
        
        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);
    }
}