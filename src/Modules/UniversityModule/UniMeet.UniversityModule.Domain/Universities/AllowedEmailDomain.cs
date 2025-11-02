namespace UniMeet.UniversityModule.Domain.Universities;

public sealed class AllowedEmailDomain
{
    public int Id { get; set; }
    public string Domain { get; set; } = null!;
    public University University { get; set; } = null!;
    public int UniversityId { get; set; }
    
    private AllowedEmailDomain() { }
    
    public AllowedEmailDomain(string domain, University university)
    {
        Domain = domain;
        University = university;
    }
}