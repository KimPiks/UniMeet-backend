namespace UniMeet.API.Models.Requests;

public class AllowedEmailUpdateRequest
{
    public int DomainId { get; set; }
    public string? NewDomain { get; set; } 
}