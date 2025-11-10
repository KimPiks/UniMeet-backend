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
            throw new ArgumentException("Department name cannot be null or empty.", nameof(name));
        
        Name = name;
        UniversityId = universityId;
    }

    internal void Rename(string newName)
    {
        Name = newName;
    }
    
    internal void AddFieldOfStudy(string name)
    {
        if (FieldsOfStudy.Any(f => f.Name == name))
            throw new InvalidOperationException($"Field of study with name '{name}' already exists in this department.");
        
        var fieldOfStudy = new FieldOfStudy(name, Id);
        FieldsOfStudy.Add(fieldOfStudy);
    }
    
    internal void RemoveFieldOfStudy(int fieldOfStudyId)
    {
        var fieldOfStudy = FieldsOfStudy.FirstOrDefault(f => f.Id == fieldOfStudyId);
        if (fieldOfStudy == null)
            throw new InvalidOperationException($"Field of study with id '{fieldOfStudy}' is not found in this department.");
        
        FieldsOfStudy.Remove(fieldOfStudy);
    }
    
    internal void RenameFieldOfStudy(int fieldOfStudyId, string newName)
    {
        var fieldOfStudy = FieldsOfStudy.FirstOrDefault(f => f.Id == fieldOfStudyId);
        if (fieldOfStudy == null)
            throw new InvalidOperationException($"Field of study with id '{fieldOfStudyId}' is not found in this department.");
        
        if (FieldsOfStudy.Any(f => f.Name == newName))
            throw new InvalidOperationException($"Field of study with name '{newName}' already exists in this department.");
        
        fieldOfStudy.Rename(newName);
    }
}