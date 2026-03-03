namespace UniMeet.API.Models.Requests;

public record UpdateUserDetailRequest(int UserDetailId, List<int>? InterestIds);

