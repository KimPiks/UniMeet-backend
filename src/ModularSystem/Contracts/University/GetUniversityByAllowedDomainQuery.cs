using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University;

public record GetUniversityByAllowedDomainQuery(string Domain) : IQuery<UniversityLookupDto?>;
