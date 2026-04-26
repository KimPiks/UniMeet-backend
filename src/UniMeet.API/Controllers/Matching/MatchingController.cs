using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Attributes;
using UniMeet.API.Responses;
using UniMeet.MatchingModule.Application.Likes;
using UniMeet.MatchingModule.Application.Likes.LikeUser;
using UniMeet.MatchingModule.Application.Matches;
using UniMeet.MatchingModule.Application.Matches.GetUserMatches;
using UniMeet.MatchingModule.Application.Matches.Unmatch;
using UniMeet.MessagingModule.Application.Conversations.CreateConversation;
using UniMeet.Shared.Abstractions;

namespace UniMeet.API.Controllers.Matching;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
[ActiveUser]
public class MatchingController(IMediator mediator) : ControllerBase
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

    [HttpPost]
    [Permission("MatchingModule.Unmatch")]
    public async Task<IActionResult> Unmatch([FromQuery] Guid otherUserId)
    {
        await mediator.SendAsync(new UnmatchCommand(CurrentUserId, otherUserId));
        return Ok(ApiResponse<string>.Ok(null, "Unmatched"));
    }
}

public record LikeUserRequest(Guid OtherUserId);
