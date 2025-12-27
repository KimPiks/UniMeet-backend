namespace UniMeet.API.Models.Requests;

public class SetGroupRequest
{
    public Guid UserId { get; set; }
    public int GroupId { get; set; }
}