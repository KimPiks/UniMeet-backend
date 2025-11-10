using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.AddFieldOfStudy;

public record AddFieldOfStudyCommand(int UniversityId, int DepartmentId, string FieldOfStudyName) : IRequest;