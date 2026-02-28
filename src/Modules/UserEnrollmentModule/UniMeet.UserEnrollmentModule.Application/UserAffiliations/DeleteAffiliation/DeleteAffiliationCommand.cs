using UniMeet.Shared.Abstractions;

namespace UniMeet.UserEnrollmentModule.Application.UserAffiliations.DeleteAffiliation;

public record DeleteAffiliationCommand(int AffiliationId) : ICommand;