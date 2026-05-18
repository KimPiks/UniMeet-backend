using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.UserEnrollment;

public record GetFieldOfStudyIdsByUserIdsQuery(IReadOnlyCollection<Guid> UserIds)
    : IQuery<IDictionary<Guid, int>>;
