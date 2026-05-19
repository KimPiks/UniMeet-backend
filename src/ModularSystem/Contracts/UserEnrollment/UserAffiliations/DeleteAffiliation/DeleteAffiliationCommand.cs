using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.UserEnrollment.UserAffiliations.DeleteAffiliation;

public record DeleteAffiliationCommand(int AffiliationId) : ICommand;