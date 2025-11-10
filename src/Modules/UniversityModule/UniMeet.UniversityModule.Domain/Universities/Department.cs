namespace UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;

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
            throw new ArgumentException("Department name cannot be null or empty.", nameof(name));
        
        Name = name;
        UniversityId = universityId;
    }

    internal void Rename(string newName)
    {
        Name = newName;
    }
    
    internal FieldOfStudy AddFieldOfStudy(string name, int departmentId)
    {
        if (FieldsOfStudy.Any(f => f.Name == name))
        {
            throw new InvalidOperationException($"Field of study with name '{name}' already exists in this department.");
        }
        
        var fieldOfStudy = new FieldOfStudy(name, departmentId);
        FieldsOfStudy.Add(fieldOfStudy);
        return fieldOfStudy;
    }
    
    internal void RemoveFieldOfStudy(string name)
    {
        var fieldOfStudy = FieldsOfStudy.FirstOrDefault(f => f.Name == name);
        if (fieldOfStudy == null)
        {
            throw new InvalidOperationException($"Field of study with name '{name}' is not found in this department.");
        }
        
        FieldsOfStudy.Remove(fieldOfStudy);
    }
    
    internal void RenameFieldOfStudy(string currentName, string newName)
    {
        var fieldOfStudy = FieldsOfStudy.FirstOrDefault(f => f.Name == currentName);
        if (fieldOfStudy == null)
        {
            throw new InvalidOperationException($"Field of study with name '{currentName}' is not found in this department.");
        }
        
        if (FieldsOfStudy.Any(f => f.Name == newName))
        {
            throw new InvalidOperationException($"Field of study with name '{newName}' already exists in this department.");
        }
        
        fieldOfStudy.Rename(newName);
    }
}