namespace UniMeet.API.Models.Requests;

public class EnrollInCourseRequest
{
    public required Guid UserId { get; set; }
    public required int FieldOfStudyId { get; set; }
}