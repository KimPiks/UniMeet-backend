namespace UniMeet.API.Models.Requests;

public class FieldOfStudyUpdateRequest
{
    public required int FieldOfStudyId { get; set; }
    public required string FieldOfStudyName { get; set; }
}