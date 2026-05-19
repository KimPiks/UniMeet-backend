using UniMeet.UserModule.Domain.ConfirmationCodes;
using UniMeet.UserModule.Domain.Interests;
using UniMeet.UserModule.Domain.PasswordResetCodes;
using UniMeet.UserModule.Domain.RefreshTokens;
using UniMeet.UserModule.Domain.UserDetails;
using UniMeet.UserModule.Domain.Users;
using UniMeet.UserModule.Infrastructure.ConfirmationCodes;
using UniMeet.UserModule.Infrastructure.Interests;
using UniMeet.UserModule.Infrastructure.PasswordResetCodes;
using UniMeet.UserModule.Infrastructure.RefreshTokens;
using UniMeet.UserModule.Infrastructure.UserDetails;
using UniMeet.UserModule.Infrastructure.Users;

namespace UniMeet.UnitTests.Infrastructure;

public class UserRepositoryTests
{
    [Fact]
    public async Task UserRepository_adds_loads_pages_searches_and_deletes_users()
    {
        await using var context = RepositoryTestContextFactory.CreateUserContext();
        var repository = new UserRepository(context);
        var chess = new Interest { Id = 1, Name = "Chess" };
        var activeUser = CreateUser("active@uni.edu", 1, Sex.Female);
        activeUser.Activate();
        activeUser.UserDetail.AddInterest(chess);
        var inactiveUser = CreateUser("inactive@uni.edu", 1, Sex.Female);

        await repository.AddAsync(activeUser);
        await repository.AddAsync(inactiveUser);
        await repository.SaveChangesAsync();
        var byId = await repository.GetByIdAsync(activeUser.Id);
        var byEmail = await repository.GetByEmailAsync("active@uni.edu");
        var paged = (await repository.GetAllAsync(0, 10)).ToList();
        var searchByFilters = await repository.SearchAsync(1, Sex.Female, [1], [activeUser.Id]);
        var emptySearch = await repository.SearchAsync(null, null, null, []);
        repository.Delete(inactiveUser);
        await repository.SaveChangesAsync();

        Assert.Same(activeUser, byId);
        Assert.Same(activeUser, byEmail);
        Assert.Equal(2, paged.Count);
        Assert.Equal([activeUser.Id], searchByFilters.Select(user => user.Id).ToArray());
        Assert.Empty(emptySearch);
        Assert.Null(await repository.GetByIdAsync(inactiveUser.Id));
    }

    [Fact]
    public async Task InterestRepository_adds_loads_pages_and_deletes_interest()
    {
        await using var context = RepositoryTestContextFactory.CreateUserContext();
        var repository = new InterestRepository(context);
        var interest = new Interest { Name = "Chess" };

        await repository.AddAsync(interest);
        await repository.SaveChangesAsync();
        var byId = await repository.GetByIdAsync(interest.Id);
        var all = (await repository.GetAllAsync(0, 10)).Single();
        repository.Delete(all);
        await repository.SaveChangesAsync();

        Assert.Same(interest, byId);
        Assert.Empty(await repository.GetAllAsync(0, 10));
    }

    [Fact]
    public async Task UserDetailRepository_loads_details_with_user_and_interests()
    {
        await using var context = RepositoryTestContextFactory.CreateUserContext();
        var user = CreateUser("detail@uni.edu", 1, Sex.Male);
        user.UserDetail.AddInterest(new Interest { Name = "Math" });
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var repository = new UserDetailRepository(context);

        var byId = await repository.GetByIdAsync(user.UserDetail.Id);
        var byUserId = await repository.GetByUserIdAsync(user.Id);
        var all = (await repository.GetAllAsync(0, 10)).Single();

        Assert.NotNull(byId);
        Assert.Equal(user.Id, byId.User.Id);
        Assert.Single(byUserId!.Interests);
        Assert.Equal(user.UserDetail.Id, all.Id);
    }

    [Fact]
    public async Task Token_and_code_repositories_add_load_and_delete_entities()
    {
        await using var context = RepositoryTestContextFactory.CreateUserContext();
        var user = CreateUser("codes@uni.edu", 1, Sex.Male);
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var refreshRepository = new RefreshTokenRepository(context);
        var confirmationRepository = new ConfirmationCodeRepository(context);
        var resetRepository = new PasswordResetCodeRepository(context);
        var refreshToken = new RefreshToken("refresh-token", DateTime.UtcNow.AddHours(1), user.Id);
        var confirmationCode = new ConfirmationCode(user.Id, DateTime.UtcNow.AddHours(1));
        var passwordResetCode = new PasswordResetCode(user.Id, DateTime.UtcNow.AddHours(1));

        await refreshRepository.AddAsync(refreshToken);
        await confirmationRepository.AddAsync(confirmationCode);
        await resetRepository.AddAsync(passwordResetCode);
        await refreshRepository.SaveChangesAsync();
        var refreshByToken = await refreshRepository.GetByTokenAsync("refresh-token");
        var confirmationByCode = await confirmationRepository.GetByCodeAsync(confirmationCode.Code);
        var resetByCode = await resetRepository.GetByCodeAsync(passwordResetCode.Code);
        refreshRepository.Delete(refreshToken);
        confirmationRepository.Delete(confirmationCode);
        resetRepository.Delete(passwordResetCode);
        await refreshRepository.SaveChangesAsync();

        Assert.NotNull(refreshByToken);
        Assert.Equal(user.Id, refreshByToken.User.Id);
        Assert.NotNull(confirmationByCode);
        Assert.Equal(user.Id, confirmationByCode.User.Id);
        Assert.NotNull(resetByCode);
        Assert.Equal(user.Id, resetByCode.User.Id);
        Assert.Empty(await refreshRepository.GetAllAsync(0, 10));
        Assert.Empty(await confirmationRepository.GetAllAsync(0, 10));
        Assert.Empty(await resetRepository.GetAllAsync(0, 10));
    }

    private static User CreateUser(string email, int universityId, Sex sex)
        => new("Anna", "Kowalska", email, "hash", universityId, 3, sex);
}
