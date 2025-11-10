namespace UniMeet.API.Models.Requests;

public class UniversityUpdateRequest
{
    public int UniversityId { get; set; }
    public string? Name { get; set; }
    public string? Country { get; set; }
    public string? Voivodeship { get; set; }
    public string? City { get; set; }
    public string? Address { get; set; }
}