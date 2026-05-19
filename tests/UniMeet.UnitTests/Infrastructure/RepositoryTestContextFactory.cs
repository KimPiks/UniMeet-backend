using Microsoft.EntityFrameworkCore;
using PermissionsModule.Infrastructure;
using UniMeet.MatchingModule.Infrastructure;
using UniMeet.MessagingModule.Infrastructure;
using UniMeet.UniversityModule.Infrastructure;
using UniMeet.UserEnrollmentModule.Infrastructure;
using UniMeet.UserModule.Infrastructure;

namespace UniMeet.UnitTests.Infrastructure;

internal static class RepositoryTestContextFactory
{
    public static MatchingContext CreateMatchingContext()
        => new(CreateOptions<MatchingContext>());

    public static MessagingContext CreateMessagingContext()
        => new(CreateOptions<MessagingContext>());

    public static PermissionsContext CreatePermissionsContext()
        => new(CreateOptions<PermissionsContext>());

    public static UniversityContext CreateUniversityContext()
        => new(CreateOptions<UniversityContext>());

    public static UserEnrollmentContext CreateUserEnrollmentContext()
        => new(CreateOptions<UserEnrollmentContext>());

    public static UserContext CreateUserContext()
        => new(CreateOptions<UserContext>());

    private static DbContextOptions<TContext> CreateOptions<TContext>()
        where TContext : DbContext
    {
        return new DbContextOptionsBuilder<TContext>()
            .UseInMemoryDatabase($"{typeof(TContext).Name}-{Guid.NewGuid()}")
            .EnableSensitiveDataLogging()
            .Options;
    }
}
