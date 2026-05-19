using ModularSystem.Contracts.Mailing;
using ModularSystem.Contracts.Permissions.Permissions;
using ModularSystem.Contracts.Permissions.Permissions.GetPermissionsForGroup;
using ModularSystem.Contracts.User.Interests.DeleteInterest;
using ModularSystem.Contracts.User.ConfirmationCodes.CreateConfirmationCode;
using ModularSystem.Contracts.User.Interests.CreateInterest;
using ModularSystem.Contracts.User.Interests.GetAllInterests;
using ModularSystem.Contracts.User.Interests.GetInterestById;
using ModularSystem.Contracts.User.PasswordResetCodes.CheckIfResetPasswordCodeExists;
using ModularSystem.Contracts.User.PasswordResetCodes.RequestPasswordReset;
using ModularSystem.Contracts.User.PasswordResetCodes.ResetPassword;
using ModularSystem.Contracts.User.RefreshTokens.RefreshTokens;
using ModularSystem.Contracts.User.Users.ConfirmAccount;
using ModularSystem.Contracts.User.Users.GetAllUsers;
using ModularSystem.Contracts.User.Users.GetUserAccessInfo;
using ModularSystem.Contracts.User.Users.GetUserById;
using ModularSystem.Contracts.User.Users.LoginUser;
using ModularSystem.Contracts.User.Users.Logout;
using ModularSystem.Contracts.User.Users.SetGroup;
using UniMeet.UserModule.Application.Interests.DeleteInterest;
using UniMeet.UserModule.Application.Interests.GetInterestById;
using UniMeet.UserModule.Application.PasswordResetCodes.CheckIfResetPasswordCodeExists;
using UniMeet.UserModule.Application.PasswordResetCodes.RequestPasswordReset;
using UniMeet.UserModule.Application.PasswordResetCodes.ResetPassword;
using UniMeet.UserModule.Application.RefreshTokens.RefreshTokens;
using UniMeet.UserModule.Application.Users.ConfirmAccount;
using UniMeet.UserModule.Application.Users.GetAllUsers;
using UniMeet.UserModule.Application.Users.GetUserAccessInfo;
using UniMeet.UserModule.Application.Users.LoginUser;
using UniMeet.UserModule.Application.Users.Logout;
using UniMeet.UserModule.Application.ConfirmationCodes.CreateConfirmationCode;
using UniMeet.UserModule.Application.Interests.CreateInterest;
using UniMeet.UserModule.Application.Interests.GetAllInterests;
using UniMeet.UserModule.Application.Users.GetUserById;
using UniMeet.UserModule.Application.Users.SetGroup;
using UniMeet.UserModule.Domain.ConfirmationCodes;
using UniMeet.UserModule.Domain.Interests;
using UniMeet.UserModule.Domain.Models;
using UniMeet.UserModule.Domain.PasswordResetCodes;
using UniMeet.UserModule.Domain.RefreshTokens;
using UniMeet.UserModule.Domain.Services;
using UniMeet.UserModule.Domain.UserDetails;
using UniMeet.UserModule.Domain.Users;
using UniMeet.UserModule.Domain.Users.Exceptions;
using UniMeet.UserModule.Infrastructure.ConfirmationCodes;
using UniMeet.UserModule.Infrastructure.Interests;
using UniMeet.UserModule.Infrastructure.PasswordResetCodes;
using UniMeet.UserModule.Infrastructure.RefreshTokens;
using UniMeet.UserModule.Infrastructure.Users;

namespace UniMeet.UnitTests.Cqrs;

public class UserHandlersTests
{
    [Fact]
    public async Task CreateInterestCommandHandler_persists_and_returns_interest()
    {
        await using var context = RepositoryTestContextFactory.CreateUserContext();
        var repository = new InterestRepository(context);
        var handler = new CreateInterestCommandHandler(repository);

        var result = await handler.HandleAsync(new CreateInterestCommand("Chess"));

        Assert.Equal("Chess", result.Name);
        Assert.Single(context.Interests);
    }

    [Fact]
    public async Task GetAllInterestsQueryHandler_returns_paged_dtos()
    {
        await using var context = RepositoryTestContextFactory.CreateUserContext();
        context.Interests.AddRange(
            new Interest { Name = "Chess" },
            new Interest { Name = "Math" });
        await context.SaveChangesAsync();
        var handler = new GetAllInterestsQueryHandler(new InterestRepository(context));

        var result = (await handler.HandleAsync(new GetAllInterestsQuery(1, 1))).ToList();

        var dto = Assert.Single(result);
        Assert.Equal("Math", dto.Name);
    }

    [Fact]
    public async Task SetGroupCommandHandler_sets_group_or_throws_when_user_is_missing()
    {
        await using var context = RepositoryTestContextFactory.CreateUserContext();
        var repository = new UserRepository(context);
        var user = CreateUser();
        await repository.AddAsync(user);
        await repository.SaveChangesAsync();
        var handler = new SetGroupCommandHandler(repository);

        await handler.HandleAsync(new SetGroupCommand(user.Id, 9));
        var exception = await Assert.ThrowsAsync<UserWithIdNotFoundException>(() =>
            handler.HandleAsync(new SetGroupCommand(Guid.NewGuid(), 9)));

        Assert.Equal(9, user.GroupId);
        Assert.Contains("not found", exception.Message);
    }

    [Fact]
    public async Task GetUserByIdQueryHandler_returns_user_dto_or_null()
    {
        await using var context = RepositoryTestContextFactory.CreateUserContext();
        var repository = new UserRepository(context);
        var user = CreateUser();
        await repository.AddAsync(user);
        await repository.SaveChangesAsync();
        var handler = new GetUserByIdQueryHandler(repository);

        var existing = await handler.HandleAsync(new GetUserByIdQuery(user.Id));
        var missing = await handler.HandleAsync(new GetUserByIdQuery(Guid.NewGuid()));

        Assert.NotNull(existing);
        Assert.Equal(user.Email, existing.Email);
        Assert.Null(missing);
    }

    [Fact]
    public async Task CreateConfirmationCodeCommandHandler_persists_code_and_returns_value()
    {
        await using var context = RepositoryTestContextFactory.CreateUserContext();
        var repository = new ConfirmationCodeRepository(context);
        var userId = Guid.NewGuid();
        var handler = new CreateConfirmationCodeCommandHandler(repository);

        var code = await handler.HandleAsync(new CreateConfirmationCodeCommand(userId));

        var persisted = Assert.Single(context.ConfirmationCodes);
        Assert.Equal(userId, persisted.UserId);
        Assert.Equal(code, persisted.Code);
        Assert.False(persisted.IsExpired());
    }

    [Fact]
    public async Task Interest_query_and_delete_handlers_return_and_remove_interest()
    {
        await using var context = RepositoryTestContextFactory.CreateUserContext();
        var repository = new InterestRepository(context);
        var interest = new Interest { Name = "Chess" };
        context.Interests.Add(interest);
        await context.SaveChangesAsync();

        var found = await new GetInterestByIdQueryHandler(repository)
            .HandleAsync(new GetInterestByIdQuery(interest.Id));
        await new DeleteInterestCommandHandler(repository)
            .HandleAsync(new DeleteInterestCommand(interest.Id));

        Assert.NotNull(found);
        Assert.Equal("Chess", found.Name);
        Assert.Empty(context.Interests);
    }

    [Fact]
    public async Task User_query_handlers_map_users_and_access_info()
    {
        await using var context = RepositoryTestContextFactory.CreateUserContext();
        var repository = new UserRepository(context);
        var user = CreateUser();
        user.Activate();
        await repository.AddAsync(user);
        await repository.SaveChangesAsync();

        var users = (await new GetAllUsersQueryHandler(repository)
            .HandleAsync(new GetAllUsersQuery(0, 10))).ToList();
        var accessInfo = await new GetUserAccessInfoQueryHandler(repository)
            .HandleAsync(new GetUserAccessInfoQuery(user.Id));
        var missing = await new GetUserAccessInfoQueryHandler(repository)
            .HandleAsync(new GetUserAccessInfoQuery(Guid.NewGuid()));

        var dto = Assert.Single(users);
        Assert.Equal(user.Email, dto.Email);
        Assert.Equal(new UserAccessInfoDto(user.Id, true, user.GroupId), accessInfo);
        Assert.Null(missing);
    }

    [Fact]
    public async Task ConfirmAccountCommandHandler_activates_user_and_removes_confirmation_code()
    {
        await using var context = RepositoryTestContextFactory.CreateUserContext();
        var user = CreateUser();
        var code = new ConfirmationCode(user.Id, DateTime.UtcNow.AddHours(1)) { User = user };
        context.Users.Add(user);
        context.ConfirmationCodes.Add(code);
        await context.SaveChangesAsync();
        var handler = new ConfirmAccountCommandHandler(new ConfirmationCodeRepository(context));

        await handler.HandleAsync(new ConfirmAccountCommand(code.Code));

        Assert.True(user.IsActive);
        Assert.Empty(context.ConfirmationCodes);
    }

    [Fact]
    public async Task LoginUserCommandHandler_creates_refresh_token_and_returns_jwt_tokens()
    {
        await using var context = RepositoryTestContextFactory.CreateUserContext();
        var userRepository = new UserRepository(context);
        var refreshTokenRepository = new RefreshTokenRepository(context);
        var user = CreateUser();
        user.Activate();
        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();
        var mediator = new FakeMediator();
        mediator.QueueResult<IEnumerable<PermissionDto>>([
            new PermissionDto { Id = 1, PermissionName = "Users.Read", CreatedAtUtc = DateTime.UtcNow }
        ]);
        var jwt = new FakeJwtService(user.Id);
        var handler = new LoginUserCommandHandler(userRepository, refreshTokenRepository, new FakePasswordHasher(), jwt, mediator);

        var tokens = await handler.HandleAsync(new LoginUserCommand(user.Email, "password"));

        Assert.Equal("access-token", tokens.AccessToken);
        Assert.Equal("refresh-token", tokens.RefreshToken);
        Assert.Single(context.RefreshTokens);
        var query = Assert.IsType<GetPermissionsForGroupQuery>(Assert.Single(mediator.SentRequests));
        Assert.Equal(user.GroupId, query.GroupId);
    }

    [Fact]
    public async Task RefreshTokensCommandHandler_replaces_valid_refresh_token()
    {
        await using var context = RepositoryTestContextFactory.CreateUserContext();
        var userRepository = new UserRepository(context);
        var refreshTokenRepository = new RefreshTokenRepository(context);
        var user = CreateUser();
        user.Activate();
        context.Users.Add(user);
        context.RefreshTokens.Add(new RefreshToken("old-refresh", DateTime.UtcNow.AddHours(1), user.Id));
        await context.SaveChangesAsync();
        var mediator = new FakeMediator();
        mediator.QueueResult<IEnumerable<PermissionDto>>([]);
        var handler = new RefreshTokensCommandHandler(refreshTokenRepository, userRepository, new FakeJwtService(user.Id), mediator);

        var tokens = await handler.HandleAsync(new RefreshTokensCommand("old-refresh"));

        Assert.Equal("access-token", tokens.AccessToken);
        Assert.Equal("refresh-token", tokens.RefreshToken);
        Assert.DoesNotContain(context.RefreshTokens, token => token.Token == "old-refresh");
        Assert.Contains(context.RefreshTokens, token => token.Token == "refresh-token");
    }

    [Fact]
    public async Task LogoutCommandHandler_removes_existing_refresh_token()
    {
        await using var context = RepositoryTestContextFactory.CreateUserContext();
        var user = CreateUser();
        context.Users.Add(user);
        context.RefreshTokens.Add(new RefreshToken("refresh-token", DateTime.UtcNow.AddHours(1), user.Id));
        await context.SaveChangesAsync();
        var handler = new LogoutCommandHandler(new RefreshTokenRepository(context), new FakeJwtService(user.Id));

        await handler.HandleAsync(new LogoutCommand("refresh-token"));

        Assert.Empty(context.RefreshTokens);
    }

    [Fact]
    public async Task Password_reset_handlers_create_check_and_consume_reset_code()
    {
        await using var context = RepositoryTestContextFactory.CreateUserContext();
        var user = CreateUser();
        user.Activate();
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var userRepository = new UserRepository(context);
        var resetRepository = new PasswordResetCodeRepository(context);
        var mediator = new FakeMediator();
        var requestHandler = new RequestPasswordResetCommandHandler(
            userRepository,
            resetRepository,
            new FakePasswordResetLinkService(),
            mediator);

        await requestHandler.HandleAsync(new RequestPasswordResetCommand(user.Email));
        var resetCode = Assert.Single(context.PasswordResetCodes);
        var exists = await new CheckIfResetPasswordCodeExistsQueryHandler(resetRepository)
            .HandleAsync(new CheckIfResetPasswordCodeExistsQuery(resetCode.Code));
        await new ResetPasswordCommandHandler(resetRepository, new FakePasswordHasher())
            .HandleAsync(new ResetPasswordCommand(resetCode.Code, "NewPassw0rd"));

        Assert.True(exists);
        Assert.Equal("hashed:NewPassw0rd", user.PasswordHash);
        Assert.Empty(context.PasswordResetCodes);
        var email = Assert.IsType<SendEmailCommand>(Assert.Single(mediator.SentRequests));
        Assert.Equal(EmailType.PasswordReset, email.EmailType);
        Assert.Equal(user.Email, email.To);
    }

    private static User CreateUser()
        => new("Anna", "Kowalska", "anna@uni.edu", "hash", 1, 3, Sex.Female);

    private sealed class FakePasswordHasher : IPasswordHasher
    {
        public string Hash(string password) => $"hashed:{password}";

        public bool Verify(string password, string hash) => password == "password" && hash == "hash";
    }

    private sealed class FakeJwtService(Guid userId) : IJwtService
    {
        public string GenerateAccessToken(Guid userId, bool isActive, int groupId, IEnumerable<string> permissions)
            => "access-token";

        public string GenerateRefreshToken(Guid userId)
            => "refresh-token";

        public Guid GetUserIdFromToken(string token, TokenType expectedTokenType)
            => userId;

        public DateTime GetExpires(TokenType tokenType)
            => DateTime.UtcNow.AddHours(1);
    }

    private sealed class FakePasswordResetLinkService : IPasswordResetLinkService
    {
        public string Create(Guid passwordResetCode)
            => $"https://example.test/reset?code={passwordResetCode}";
    }
}
