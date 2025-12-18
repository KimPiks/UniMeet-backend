using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UniMeet.UserEnrollmentModule.Infrastructure.UserAffiliation.Configuration;

public class UserAffiliationConfiguration : IEntityTypeConfiguration<Domain.UserAffiliation.UserAffiliation>
{
    public void Configure(EntityTypeBuilder<Domain.UserAffiliation.UserAffiliation> builder)
    {
        builder.ToTable("user_affiliations");
        
        builder.HasKey(ua => ua.Id);
    }
}