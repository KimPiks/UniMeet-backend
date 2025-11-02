namespace UniMeet.UniversityModule.Domain.Universities;

public sealed class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public University University { get; set; } = null!;
    public int UniversityId { get; set; }
    public ICollection<FieldOfStudy> FieldsOfStudy { get; set; } = new List<FieldOfStudy>();
    
    private Department() { }
    
    public Department(string name, University university)
    {
        Name = name;
        University = university;
    }
}