using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PermissionsModule.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionSecurityIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_permissions_GroupId",
                table: "permissions");

            migrationBuilder.CreateIndex(
                name: "IX_groups_Name",
                table: "groups",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_permissions_GroupId_PermissionName",
                table: "permissions",
                columns: new[] { "GroupId", "PermissionName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_groups_Name",
                table: "groups");

            migrationBuilder.DropIndex(
                name: "IX_permissions_GroupId_PermissionName",
                table: "permissions");

            migrationBuilder.CreateIndex(
                name: "IX_permissions_GroupId",
                table: "permissions",
                column: "GroupId");
        }
    }
}
