using MailingModule.Commands;
using MailingModule.Enums;
using MailingModule.Models;
using PermissionsModule.Application.Groups.GetGroupByName;
using PermissionsModule.Domain.Groups.Exceptions;
using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Application.Universities.GetByAllowedDomain;
using UniMeet.UserModule.Application.ConfirmationCodes.CreateConfirmationCode;
using UniMeet.UserModule.Domain.Services;
using UniMeet.UserModule.Domain.Users;
using UniMeet.UserModule.Domain.Users.Exceptions;

namespace UniMeet.UserModule.Application.Users.RegisterUser;

public class RegisterUserCommandHandler(IUserRepository userRepository, 
    IPasswordHasher passwordHasher,
    IMediator mediator,
    IConfirmationLinkService confirmationLinkService)
    : ICommandHandler<RegisterUserCommand> 
{
    public async Task HandleAsync(RegisterUserCommand request, CancellationToken cancellationToken = default)
    {
        request.Validate();
        
        // Check if user with given email already exists
        var userAlreadyExists = await userRepository.GetByEmailAsync(request.Email, cancellationToken) != null;
        if (userAlreadyExists)
        {
            throw new UserAlreadyExistsException(request.Email);
        }

        // Check if a user has a university email
        var emailDomain = request.Email.Split("@")[1];
        var university = await mediator.SendAsync(new GetByAllowedDomainQuery(emailDomain), cancellationToken);
        if (university == null)
            throw new EmailDomainNotAllowedException(request.Email);
        
        // Get default group
        var group = await mediator.SendAsync(new GetGroupByNameQuery("User"), cancellationToken);
        if (group == null)
        {
            throw new GroupNotFoundException("User");
        }
        
        // Create user
        var passwordHash = passwordHasher.Hash(request.Password);
        var user = new User(request.FirstName, request.LastName, request.Email, passwordHash, university.Id, group.Id, request.Sex);
        
        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        // Create confirmation code
        var confirmationCode = await mediator.SendAsync(new CreateConfirmationCodeCommand(user.Id), cancellationToken);
        var confirmationLink = confirmationLinkService.Create(confirmationCode);
        
        // Send confirmation email
        var emailParams = new List<EmailParameter>()
        {
            new EmailParameter("Name", user.FirstName),
            new EmailParameter("ConfirmationLink", confirmationLink)
        };
        
        var sendEmailCommand = new SendEmailCommand(user.Email, EmailType.RegisterConfirmation, emailParams);
        await mediator.SendAsync(sendEmailCommand, cancellationToken);
    }
}