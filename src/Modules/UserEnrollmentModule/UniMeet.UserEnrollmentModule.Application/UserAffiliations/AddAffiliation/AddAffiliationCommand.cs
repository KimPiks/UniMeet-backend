using UniMeet.Shared.Abstractions;

namespace UniMeet.UserEnrollmentModule.Application.UserAffiliations.AddAffiliation;

public record AddAffiliationCommand(Guid UserId, int FieldOfStudyId) : ICommand;