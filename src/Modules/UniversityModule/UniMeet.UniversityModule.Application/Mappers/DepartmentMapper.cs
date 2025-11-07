using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;

namespace UniMeet.UniversityModule.Application.Mappers;

public static class DepartmentMapper
{
    public static DepartmentDto ToDto(this Department department)
    {
        return new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name,
            FieldsOfStudy = department.FieldsOfStudy.Select(fos => fos.ToDto()).ToList()
        };
    }
}