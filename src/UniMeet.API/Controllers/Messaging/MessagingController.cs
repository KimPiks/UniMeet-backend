using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Attributes;
using UniMeet.API.Responses;
using UniMeet.MessagingModule.Application.Conversations;
using UniMeet.MessagingModule.Application.Conversations.GetOrCreateConversation;
using UniMeet.MessagingModule.Application.Conversations.GetUserConversations;
using UniMeet.MessagingModule.Application.Messages;
using UniMeet.MessagingModule.Application.Messages.GetConversationMessages;
using UniMeet.MessagingModule.Application.Messages.MarkMessagesAsRead;
using UniMeet.MessagingModule.Application.Messages.SendMessage;
using UniMeet.Shared.Abstractions;

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
    [Permission("MessagingModule.GetOrCreateConversation")]
    public async Task<IActionResult> GetOrCreateConversation([FromQuery] Guid otherUserId)
    {
        var command = new GetOrCreateConversationCommand(CurrentUserId, otherUserId);
        var conversation = await mediator.SendAsync(command);
        return Ok(ApiResponse<ConversationDto>.Ok(conversation, "Conversation retrieved"));
    }

    [HttpGet]
    [Permission("MessagingModule.GetConversations")]
    public async Task<IActionResult> GetConversations()
    {
        var query = new GetUserConversationsQuery(CurrentUserId);
        var conversations = await mediator.SendAsync(query);
        return Ok(ApiResponse<IEnumerable<ConversationDto>>.Ok(conversations, "Conversations retrieved"));
    }

    [HttpGet]
    [Permission("MessagingModule.GetMessages")]
    public async Task<IActionResult> GetMessages(
        [FromQuery] Guid conversationId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 50)
    {
        var query = new GetConversationMessagesQuery(conversationId, offset, limit);
        var messages = await mediator.SendAsync(query);
        return Ok(ApiResponse<IEnumerable<MessageDto>>.Ok(messages, "Messages retrieved"));
    }

    [HttpPost]
    [Permission("MessagingModule.SendMessage")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        var command = new SendMessageCommand(CurrentUserId, request.ConversationId, request.Content);
        var message = await mediator.SendAsync(command);
        return Ok(ApiResponse<MessageDto>.Ok(message, "Message sent"));
    }

    [HttpPost]
    [Permission("MessagingModule.MarkMessagesAsRead")]
    public async Task<IActionResult> MarkAsRead([FromQuery] Guid conversationId)
    {
        var command = new MarkMessagesAsReadCommand(conversationId, CurrentUserId);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<string>.Ok(null, "Messages marked as read"));
    }
}

public record SendMessageRequest(Guid ConversationId, string Content);
