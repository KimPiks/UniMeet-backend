using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.GetFieldOfStudyById;

public record GetFieldOfStudyByIdQuery(int FieldOfStudyId) : IQuery<FieldOfStudyDto?>;