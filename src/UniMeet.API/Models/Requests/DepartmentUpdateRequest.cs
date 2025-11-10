namespace UniMeet.API.Models.Requests;

public class DepartmentUpdateRequest
{
    public required int DepartmentId { get; set; }
    public required string DepartmentName { get; set; }
}