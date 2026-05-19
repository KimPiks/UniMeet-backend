using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.UserEnrollment.UserAffiliations.AddAffiliation;

public record AddAffiliationCommand(Guid UserId, int FieldOfStudyId) : ICommand;