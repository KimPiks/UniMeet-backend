namespace UniMeet.API.Models.Requests;

public class UniversityUpdateRequest
{
    public required int UniversityId { get; set; }
    public required string Name { get; set; }
    public required string Country { get; set; }
    public required string Voivodeship { get; set; }
    public required string City { get; set; }
    public required string Address { get; set; }
}