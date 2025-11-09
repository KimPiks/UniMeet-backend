using MediatR;

namespace UniMeet.UniversityModule.Application.Features.FieldsOfStudy.Commands.AddFieldOfStudy;

public record AddFieldOfStudyCommand(int UniversityId, int DepartmentId, string FieldOfStudyName) : IRequest;