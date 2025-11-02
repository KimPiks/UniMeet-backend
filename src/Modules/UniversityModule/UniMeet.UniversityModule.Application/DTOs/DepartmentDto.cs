namespace UniMeet.UniversityModule.Application.DTOs;

public record DepartmentDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public ICollection<FieldOfStudyDto> FieldsOfStudy { get; init; } = new List<FieldOfStudyDto>();
}