namespace UniMeet.API.Models.Requests;

public record UniversityCreateRequest(string Name, string Country, string Voivodeship, string City, string Address);