using MailingModule.Models;
using MailingModule.Services;
using ModularSystem.Contracts.Mailing;
using UniMeet.Shared.Abstractions;

namespace MailingModule.Commands;

public class SendEmailCommandHandler(IEmailService emailService) :
    ICommandHandler<SendEmailCommand>
{
    public Task HandleAsync(SendEmailCommand request, CancellationToken cancellationToken = default)
    {
        request.Validate();
        
        var email = new Email(request.To, request.EmailType, request.Parameters);
        emailService.SendEmail(email);
        
        return Task.CompletedTask;
    }
}
