using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniMeet.UniversityModule.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAllowedEmailDomainSecurityIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_allowed_email_domains_Domain",
                table: "allowed_email_domains",
                column: "Domain",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_allowed_email_domains_Domain",
                table: "allowed_email_domains");
        }
    }
}
