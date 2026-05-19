using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Attributes;
using UniMeet.API.Responses;
using ModularSystem.Contracts.Matching.Matches.AreUsersMatched;
using ModularSystem.Contracts.Messaging.Conversations;
using ModularSystem.Contracts.Messaging.Conversations.CreateGroupConversation;
using ModularSystem.Contracts.Messaging.Conversations.GetConversationById;
using ModularSystem.Contracts.Messaging.Conversations.GetConversationByUsers;
using ModularSystem.Contracts.Messaging.Conversations.GetUserConversations;
using ModularSystem.Contracts.Messaging.Messages;
using ModularSystem.Contracts.Messaging.Messages.GetConversationMessages;
using ModularSystem.Contracts.Messaging.Messages.MarkMessagesAsRead;
using ModularSystem.Contracts.Messaging.Messages.SendMessage;
using ModularSystem;
using UniMeet.Shared.Exceptions;

namespace UniMeet.API.Controllers.Messaging;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
[ActiveUser]
public class MessagingController(IModuleRequestDispatcher mediator) : ControllerBase
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
        var messages = await mediator.SendAsync(new GetConversationMessagesQuery(conversationId, CurrentUserId, offset, limit));
        return Ok(ApiResponse<IEnumerable<MessageDto>>.Ok(messages, "Messages retrieved"));
    }

    [HttpPost]
    [Permission("MessagingModule.CreateGroupConversation")]
    public async Task<IActionResult> CreateGroupConversation([FromBody] CreateGroupConversationRequest request)
    {
        var participantIds = request.ParticipantIds?.ToArray() ?? Array.Empty<Guid>();
        var errors = new List<string>();

        if (participantIds.Length < 2)
            errors.Add("A group conversation requires at least two invited participants.");

        if (participantIds.Contains(CurrentUserId))
            errors.Add("Creator cannot be included in participantIds.");

        if (participantIds.Distinct().Count() != participantIds.Length)
            errors.Add("participantIds cannot contain duplicates.");

        if (errors.Count > 0)
            throw new ValidationException(errors);

        foreach (var participantId in participantIds)
        {
            var matched = await mediator.SendAsync(new AreUsersMatchedQuery(CurrentUserId, participantId));
            if (!matched)
                throw new ForbiddenException("All group participants must be matched with the creator.");
        }

        var conversation = await mediator.SendAsync(new CreateGroupConversationCommand(CurrentUserId, participantIds));
        return Ok(ApiResponse<ConversationDto>.Ok(conversation, "Group conversation created"));
    }

    [HttpPost]
    [Permission("MessagingModule.SendMessage")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        var conversation = await mediator.SendAsync(new GetConversationByIdQuery(request.ConversationId, CurrentUserId))
            ?? throw new KeyNotFoundException($"Conversation {request.ConversationId} not found.");

        if (!conversation.ParticipantIds.Contains(CurrentUserId))
            throw new ForbiddenException("You are not a participant of this conversation.");

        if (!conversation.IsGroup)
        {
            if (!conversation.User1Id.HasValue || !conversation.User2Id.HasValue)
                throw new InvalidOperationException("Private conversation is missing participants.");

            var matched = await mediator.SendAsync(new AreUsersMatchedQuery(conversation.User1Id.Value, conversation.User2Id.Value));
            if (!matched)
                throw new ForbiddenException("Users must be matched to exchange messages.");
        }

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
public record CreateGroupConversationRequest(IReadOnlyCollection<Guid>? ParticipantIds);
