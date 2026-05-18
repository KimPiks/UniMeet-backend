using ModularSystem.Contracts.Mailing;
using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.PasswordResetCodes;
using UniMeet.UserModule.Domain.Services;
using UniMeet.UserModule.Domain.Users;
using UniMeet.UserModule.Domain.Users.Exceptions;

namespace UniMeet.UserModule.Application.PasswordResetCodes.RequestPasswordReset;

public class RequestPasswordResetCommandHandler(IUserRepository userRepository,
    IPasswordResetCodeRepository passwordResetCodeRepository,
    IPasswordResetLinkService passwordResetLinkService,
    IMediator mediator) : ICommandHandler<RequestPasswordResetCommand>
{
    public async Task HandleAsync(RequestPasswordResetCommand request, CancellationToken cancellationToken = default)
    {
        request.Validate(); 
        
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
            throw new UserNotFoundException(request.Email);
        
        if (!user.IsActive)
            throw new UserNotActiveException(request.Email);
        
        var codeExpiration = DateTime.UtcNow.AddDays(1);
        var passwordResetCode = new PasswordResetCode(user.Id, codeExpiration);
        await passwordResetCodeRepository.AddAsync(passwordResetCode, cancellationToken);
        await passwordResetCodeRepository.SaveChangesAsync(cancellationToken);
        
        // Send email
        var passwordResetUrl = passwordResetLinkService.Create(passwordResetCode.Code);
        var emailParams = new List<EmailParameter>()
        {
            new EmailParameter("PasswordResetLink", passwordResetUrl)
        };
        
        var sendEmailCommand = new SendEmailCommand(user.Email, EmailType.PasswordReset, emailParams);
        await mediator.SendAsync(sendEmailCommand, cancellationToken);
    }
}
