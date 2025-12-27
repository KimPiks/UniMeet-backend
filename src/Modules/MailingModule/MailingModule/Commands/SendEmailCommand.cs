using MailingModule.Enums;
using MailingModule.Models;
using UniMeet.Shared.Abstractions;

namespace MailingModule.Commands;

public record SendEmailCommand(
    string To, 
    EmailType EmailType, 
    List<EmailParameter> Parameters) 
    : ICommand;