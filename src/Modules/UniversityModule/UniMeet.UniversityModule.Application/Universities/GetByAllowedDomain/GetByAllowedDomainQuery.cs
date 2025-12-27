using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Universities.GetByAllowedDomain;

public record GetByAllowedDomainQuery(string Domain) : IQuery<UniversityDto?>;