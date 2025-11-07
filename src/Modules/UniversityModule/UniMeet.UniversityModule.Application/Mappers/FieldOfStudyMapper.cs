using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;

namespace UniMeet.UniversityModule.Application.Mappers;

public static class FieldOfStudyMapper
{
    public static FieldOfStudyDto ToDto(this FieldOfStudy fieldOfStudy)
    {
        return new FieldOfStudyDto
        {
            Id = fieldOfStudy.Id,
            Name = fieldOfStudy.Name
        };
    }
}