namespace UniMeet.API.Models.Requests;

public class PasswordResetRequest
{
    public Guid Code { get; set; }
    public string NewPassword { get; set; }
}