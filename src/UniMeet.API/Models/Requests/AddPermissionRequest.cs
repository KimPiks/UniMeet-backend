namespace UniMeet.API.Models.Requests;

public class AddPermissionRequest
{
    public int GroupId { get; set; }
    public string PermissionName { get; set; }
}