using UniMeet.UniversityModule.Domain.Universities.Exceptions;

namespace UniMeet.UniversityModule.Domain.Universities;

public sealed class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public University University { get; set; } = null!;
    public int UniversityId { get; set; }
    public ICollection<FieldOfStudy> FieldsOfStudy { get; set; } = new List<FieldOfStudy>();
    
    private Department() { }
    
    internal Department(string name, int universityId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidDepartmentNameException(name);
        
        Name = name;
        UniversityId = universityId;
    }

    internal void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new InvalidDepartmentNameException(newName);
        
        Name = newName;
    }
    
    internal void AddFieldOfStudy(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidFieldOfStudyNameException(name);
        
        if (FieldsOfStudy.Any(f => f.Name == name))
            throw new FieldOfStudyAlreadyExistsException(name); 
                
        var fieldOfStudy = new FieldOfStudy(name, Id);
        FieldsOfStudy.Add(fieldOfStudy);
    }
    
    internal void RemoveFieldOfStudy(int fieldOfStudyId)
    {
        var fieldOfStudy = FieldsOfStudy.FirstOrDefault(f => f.Id == fieldOfStudyId);
        if (fieldOfStudy == null)
            throw new FieldOfStudyNotFoundException(fieldOfStudyId);
        
        FieldsOfStudy.Remove(fieldOfStudy);
    }
    
    internal void RenameFieldOfStudy(int fieldOfStudyId, string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new InvalidFieldOfStudyNameException(newName);
        
        var fieldOfStudy = FieldsOfStudy.FirstOrDefault(f => f.Id == fieldOfStudyId);
        if (fieldOfStudy == null)
            throw new FieldOfStudyNotFoundException(fieldOfStudyId);
        
        if (FieldsOfStudy.Any(f => f.Name == newName))
            throw new FieldOfStudyAlreadyExistsException(newName);
        
        fieldOfStudy.Rename(newName);
    }
}