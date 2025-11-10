using UniMeet.UniversityModule.Domain.Universities.Exceptions;

namespace UniMeet.UniversityModule.Domain.Universities;

public sealed class FieldOfStudy
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public Department Department { get; set; } = null!;
    public int DepartmentId { get; set; }
    
    private FieldOfStudy() { }
    
    internal FieldOfStudy(string name, int departmentId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidFieldOfStudyNameException(name);
        
        Name = name;
        DepartmentId = departmentId;
    }
    
    internal void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new InvalidFieldOfStudyNameException(newName);
        
        Name = newName;
    }
}