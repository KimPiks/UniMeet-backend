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
        Domain = domain;
        UniversityId = universityId;
    }
    
    internal void ChangeDomain(string newDomain)
    {
        Domain = newDomain;
    }
}