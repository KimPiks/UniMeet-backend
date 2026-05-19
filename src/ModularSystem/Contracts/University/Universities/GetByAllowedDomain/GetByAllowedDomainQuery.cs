using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.Universities.GetByAllowedDomain;

public record GetByAllowedDomainQuery(string Domain) : IQuery<UniversityDto?>;