using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.AllowedEmailDomains.GetAllowedEmailDomainsByUniversityId;

public record GetAllowedEmailDomainsByUniversityIdQuery(int UniversityId, int Offset, int Limit) : IQuery<IEnumerable<AllowedEmailDomainDto>>;