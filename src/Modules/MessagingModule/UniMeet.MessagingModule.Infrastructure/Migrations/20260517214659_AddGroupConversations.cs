using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniMeet.MessagingModule.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupConversations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_conversations_User1Id_User2Id",
                table: "conversations");

            migrationBuilder.AlterColumn<Guid>(
                name: "User2Id",
                table: "conversations",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "User1Id",
                table: "conversations",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "conversations",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGroup",
                table: "conversations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "conversation_participants",
                columns: table => new
                {
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conversation_participants", x => new { x.ConversationId, x.UserId });
                    table.ForeignKey(
                        name: "FK_conversation_participants_conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "message_read_receipts",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message_read_receipts", x => new { x.MessageId, x.UserId });
                    table.ForeignKey(
                        name: "FK_message_read_receipts_messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql("""
                INSERT INTO conversation_participants ("ConversationId", "UserId", "JoinedAt")
                SELECT "Id", "User1Id", "CreatedAt"
                FROM conversations
                WHERE "User1Id" IS NOT NULL
                ON CONFLICT DO NOTHING;

                INSERT INTO conversation_participants ("ConversationId", "UserId", "JoinedAt")
                SELECT "Id", "User2Id", "CreatedAt"
                FROM conversations
                WHERE "User2Id" IS NOT NULL
                ON CONFLICT DO NOTHING;

                INSERT INTO message_read_receipts ("MessageId", "UserId", "ReadAt")
                SELECT
                    m."Id",
                    CASE
                        WHEN m."SenderId" = c."User1Id" THEN c."User2Id"
                        ELSE c."User1Id"
                    END,
                    m."SentAt"
                FROM messages m
                INNER JOIN conversations c ON c."Id" = m."ConversationId"
                WHERE m."IsRead" = TRUE
                  AND c."User1Id" IS NOT NULL
                  AND c."User2Id" IS NOT NULL
                  AND m."SenderId" IN (c."User1Id", c."User2Id")
                ON CONFLICT DO NOTHING;
                """);

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "messages");

            migrationBuilder.CreateIndex(
                name: "IX_conversations_User1Id_User2Id",
                table: "conversations",
                columns: new[] { "User1Id", "User2Id" },
                unique: true,
                filter: "\"IsGroup\" = FALSE AND \"User1Id\" IS NOT NULL AND \"User2Id\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_conversation_participants_UserId",
                table: "conversation_participants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_message_read_receipts_UserId",
                table: "message_read_receipts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_conversations_User1Id_User2Id",
                table: "conversations");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "messages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql("""
                UPDATE messages AS m
                SET "IsRead" = TRUE
                FROM message_read_receipts AS r
                WHERE r."MessageId" = m."Id";
                """);

            migrationBuilder.DropTable(
                name: "conversation_participants");

            migrationBuilder.DropTable(
                name: "message_read_receipts");

            migrationBuilder.Sql("""
                DELETE FROM conversations
                WHERE "IsGroup" = TRUE;
                """);

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "conversations");

            migrationBuilder.DropColumn(
                name: "IsGroup",
                table: "conversations");

            migrationBuilder.AlterColumn<Guid>(
                name: "User2Id",
                table: "conversations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "User1Id",
                table: "conversations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_conversations_User1Id_User2Id",
                table: "conversations",
                columns: new[] { "User1Id", "User2Id" },
                unique: true);
        }
    }
}
