using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Attributes;
using UniMeet.API.Responses;
using UniMeet.MatchingModule.Application.Matches.AreUsersMatched;
using UniMeet.MessagingModule.Application.Conversations;
using UniMeet.MessagingModule.Application.Conversations.GetConversationById;
using UniMeet.MessagingModule.Application.Conversations.GetConversationByUsers;
using UniMeet.MessagingModule.Application.Conversations.GetUserConversations;
using UniMeet.MessagingModule.Application.Messages;
using UniMeet.MessagingModule.Application.Messages.GetConversationMessages;
using UniMeet.MessagingModule.Application.Messages.MarkMessagesAsRead;
using UniMeet.MessagingModule.Application.Messages.SendMessage;
using UniMeet.Shared.Abstractions;
using UniMeet.Shared.Exceptions;

namespace UniMeet.API.Controllers.Messaging;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
[ActiveUser]
public class MessagingController(IMediator mediator) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

    [HttpGet]
    [Permission("MessagingModule.GetConversation")]
    public async Task<IActionResult> GetConversation([FromQuery] Guid otherUserId)
    {
        var matched = await mediator.SendAsync(new AreUsersMatchedQuery(CurrentUserId, otherUserId));
        if (!matched)
            throw new ForbiddenException("Users must be matched to access a conversation.");

        var conversation = await mediator.SendAsync(new GetConversationByUsersQuery(CurrentUserId, otherUserId));
        if (conversation is null)
            return NotFound(ApiResponse<string>.Fail(null, "Conversation not found."));

        return Ok(ApiResponse<ConversationDto>.Ok(conversation, "Conversation retrieved"));
    }

    [HttpGet]
    [Permission("MessagingModule.GetConversations")]
    public async Task<IActionResult> GetConversations()
    {
        var conversations = await mediator.SendAsync(new GetUserConversationsQuery(CurrentUserId));
        return Ok(ApiResponse<IEnumerable<ConversationDto>>.Ok(conversations, "Conversations retrieved"));
    }

    [HttpGet]
    [Permission("MessagingModule.GetMessages")]
    public async Task<IActionResult> GetMessages(
        [FromQuery] Guid conversationId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 50)
    {
        var messages = await mediator.SendAsync(new GetConversationMessagesQuery(conversationId, offset, limit));
        return Ok(ApiResponse<IEnumerable<MessageDto>>.Ok(messages, "Messages retrieved"));
    }

    [HttpPost]
    [Permission("MessagingModule.SendMessage")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        var conversation = await mediator.SendAsync(new GetConversationByIdQuery(request.ConversationId))
            ?? throw new KeyNotFoundException($"Conversation {request.ConversationId} not found.");

        if (conversation.User1Id != CurrentUserId && conversation.User2Id != CurrentUserId)
            throw new ForbiddenException("You are not a participant of this conversation.");

        var matched = await mediator.SendAsync(new AreUsersMatchedQuery(conversation.User1Id, conversation.User2Id));
        if (!matched)
            throw new ForbiddenException("Users must be matched to exchange messages.");

        var message = await mediator.SendAsync(new SendMessageCommand(CurrentUserId, request.ConversationId, request.Content));
        return Ok(ApiResponse<MessageDto>.Ok(message, "Message sent"));
    }

    [HttpPost]
    [Permission("MessagingModule.MarkMessagesAsRead")]
    public async Task<IActionResult> MarkAsRead([FromQuery] Guid conversationId)
    {
        await mediator.SendAsync(new MarkMessagesAsReadCommand(conversationId, CurrentUserId));
        return Ok(ApiResponse<string>.Ok(null, "Messages marked as read"));
    }
}

public record SendMessageRequest(Guid ConversationId, string Content);
