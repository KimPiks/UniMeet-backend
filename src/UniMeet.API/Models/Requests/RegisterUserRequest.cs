using ModularSystem.Contracts.User.UserDetails;

namespace UniMeet.API.Models.Requests;

public record RegisterUserRequest(string FirstName, string LastName, string Email, string Password, Sex Sex);