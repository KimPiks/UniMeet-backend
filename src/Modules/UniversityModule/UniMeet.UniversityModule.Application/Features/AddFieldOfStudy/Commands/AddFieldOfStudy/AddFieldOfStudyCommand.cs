using UniMeet.Shared.Abstractions;
using UniMeet.Shared.Mediator;

namespace UniMeet.UniversityModule.Application.Features.FieldsOfStudy.Commands.AddFieldOfStudy;

public record AddFieldOfStudyCommand(int UniversityId, int DepartmentId, string FieldOfStudyName) : IRequest;