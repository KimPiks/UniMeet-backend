using UniMeet.UniversityModule.Domain.Universities.Exceptions;

namespace UniMeet.UniversityModule.Domain.Universities;

public sealed class AllowedEmailDomain
{
    public int Id { get; set; }
    public string Domain { get; set; } = null!;
    public University University { get; set; } = null!;
    public int UniversityId { get; set; }
    
    private AllowedEmailDomain() { }
    
    internal AllowedEmailDomain(string domain, int universityId)
    {
        if (string.IsNullOrWhiteSpace(domain))
            throw new InvalidAllowedEmailDomainNameException(domain);
        
        Domain = domain;
        UniversityId = universityId;
    }
    
    internal void ChangeDomain(string newDomain)
    {
        if (string.IsNullOrWhiteSpace(newDomain))
            throw new InvalidAllowedEmailDomainNameException(newDomain);
        
        Domain = newDomain;
    }
}