namespace UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;

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
            throw new ArgumentException("Email domain cannot be null or empty.", nameof(domain));
        
        Domain = domain;
        UniversityId = universityId;
    }
    
    internal void ChangeDomain(string newDomain)
    {
        Domain = newDomain;
    }
}