namespace UniMeet.API.Models.Requests;

public class DepartmentUpdateRequest
{
    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
}