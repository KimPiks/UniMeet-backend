using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PermissionsModule.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreateGroupConversationPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                INSERT INTO permissions ("GroupId", "PermissionName", "CreatedAtUtc")
                SELECT g."Id", 'MessagingModule.CreateGroupConversation', CURRENT_TIMESTAMP
                FROM groups g
                WHERE g."Name" IN ('User', 'Admin')
                  AND NOT EXISTS (
                      SELECT 1
                      FROM permissions p
                      WHERE p."GroupId" = g."Id"
                        AND p."PermissionName" = 'MessagingModule.CreateGroupConversation'
                  );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DELETE FROM permissions p
                USING groups g
                WHERE p."GroupId" = g."Id"
                  AND g."Name" IN ('User', 'Admin')
                  AND p."PermissionName" = 'MessagingModule.CreateGroupConversation';
                """);
        }
    }
}
