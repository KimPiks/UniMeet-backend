using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.AllowedEmailDomains.GetAllowedEmailDomainById;

public record GetAllowedEmailDomainByIdQuery(int DomainId) : IQuery<AllowedEmailDomainDto?>;