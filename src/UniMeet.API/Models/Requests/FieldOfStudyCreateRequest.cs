namespace UniMeet.API.Models.Requests;

public record FieldOfStudyCreateRequest(int UniversityId, int DepartmentId, string FieldOfStudyName);