using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Attributes;
using UniMeet.API.Responses;
using ModularSystem.Contracts.Matching.Likes.GetLikes;
using ModularSystem.Contracts.Matching.Likes;
using ModularSystem.Contracts.Matching.Likes.LikeUser;
using ModularSystem.Contracts.Matching.Matches;
using ModularSystem.Contracts.Matching.Matches.GetUserMatches;
using ModularSystem.Contracts.Matching.Matches.Unmatch;
using ModularSystem.Contracts.Messaging.Conversations.CreateConversation;
using ModularSystem;
using ModularSystem.Contracts.User.Users;
using ModularSystem.Contracts.User.Users.GetUserById;

namespace UniMeet.API.Controllers.Matching;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
[ActiveUser]
public class MatchingController(IModuleRequestDispatcher mediator) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

    [HttpPost]
    [Permission("MatchingModule.LikeUser")]
    public async Task<IActionResult> LikeUser([FromBody] LikeUserRequest request)
    {
        var result = await mediator.SendAsync(new LikeUserCommand(CurrentUserId, request.OtherUserId));

        if (result.JustMatched)
        {
            await mediator.SendAsync(new CreateConversationCommand(CurrentUserId, request.OtherUserId));
        }

        var message = result.Matched ? "It's a match!" : "Like recorded";
        return Ok(ApiResponse<LikeResultDto>.Ok(result, message));
    }

    [HttpGet]
    [Permission("MatchingModule.GetMatches")]
    public async Task<IActionResult> GetMatches()
    {
        var matches = await mediator.SendAsync(new GetUserMatchesQuery(CurrentUserId));
        return Ok(ApiResponse<IEnumerable<MatchDto>>.Ok(matches, "Matches retrieved"));
    }

    [HttpGet]
    [Permission("MatchingModule.GetLikes")]
    public async Task<IActionResult> GetLikes([FromQuery] Guid userId)
    {
        var likedUserIds = await mediator.SendAsync(new GetLikesQuery(userId));
        var likedUsers = new List<UserDto?>();
        foreach (var id in likedUserIds)
        {
            likedUsers.Add(await mediator.SendAsync(new GetUserByIdQuery(id)));
        }

        return Ok(ApiResponse<IEnumerable<UserDto>>.Ok(
            likedUsers.Where(user => user is not null).Select(user => user!).ToList(),
            "Liked users retrieved"));
    }

    [HttpPost]
    [Permission("MatchingModule.Unmatch")]
    public async Task<IActionResult> Unmatch([FromQuery] Guid otherUserId)
    {
        await mediator.SendAsync(new UnmatchCommand(CurrentUserId, otherUserId));
        return Ok(ApiResponse<string>.Ok(null!, "Unmatched"));
    }
}

public record LikeUserRequest(Guid OtherUserId);
