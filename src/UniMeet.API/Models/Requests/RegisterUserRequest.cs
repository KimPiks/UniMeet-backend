namespace UniMeet.API.Models.Requests;

public record RegisterUserRequest(string FirstName, string LastName, string Email, string Password);