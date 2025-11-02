namespace UniMeet.UniversityModule.Domain.Universities;

public sealed class FieldOfStudy
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public Department Department { get; set; } = null!;
    public int DepartmentId { get; set; }
    
    private FieldOfStudy() { }
    
    public FieldOfStudy(string name, Department department)
    {
        Name = name;
        Department = department;
    }
}