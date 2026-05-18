using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.UserEnrollment;

public record GetUserIdsByFieldOfStudyIdQuery(int FieldOfStudyId)
    : IQuery<IReadOnlyList<Guid>>;
