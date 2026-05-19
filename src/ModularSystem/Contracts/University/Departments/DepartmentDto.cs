using ModularSystem.Contracts.University.FieldsOfStudy;

namespace ModularSystem.Contracts.University.Departments;

public record DepartmentDto
{
    public required int Id { get; init; }
    public required string Name { get; init; } = string.Empty;
    public required ICollection<FieldOfStudyDto> FieldsOfStudy { get; init; } = new List<FieldOfStudyDto>();
}