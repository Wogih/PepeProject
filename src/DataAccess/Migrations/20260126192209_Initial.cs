using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_users_AccountUserId",
                table: "RefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK__users__B9BE370F7F35AA08",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken");

            migrationBuilder.RenameTable(
                name: "RefreshToken",
                newName: "refresh_tokens");

            migrationBuilder.RenameColumn(
                name: "Verified",
                table: "users",
                newName: "verified");

            migrationBuilder.RenameColumn(
                name: "Updated",
                table: "users",
                newName: "updated");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "users",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "VerificationToken",
                table: "users",
                newName: "verification_token");

            migrationBuilder.RenameColumn(
                name: "SystemRole",
                table: "users",
                newName: "system_role");

            migrationBuilder.RenameColumn(
                name: "ResetTokenExpires",
                table: "users",
                newName: "reset_token_expires");

            migrationBuilder.RenameColumn(
                name: "ResetToken",
                table: "users",
                newName: "reset_token");

            migrationBuilder.RenameColumn(
                name: "PasswordReset",
                table: "users",
                newName: "password_reset");

            migrationBuilder.RenameColumn(
                name: "AcceptTerms",
                table: "users",
                newName: "accept_terms");

            migrationBuilder.RenameIndex(
                name: "UQ__users__F3DBC572F302E448",
                table: "users",
                newName: "UQ__users__F3DBC57296E81119");

            migrationBuilder.RenameIndex(
                name: "UQ__users__AB6E616448A45D4C",
                table: "users",
                newName: "UQ__users__AB6E616418F0B0D9");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "refresh_tokens",
                newName: "token");

            migrationBuilder.RenameColumn(
                name: "Revoked",
                table: "refresh_tokens",
                newName: "revoked");

            migrationBuilder.RenameColumn(
                name: "Expires",
                table: "refresh_tokens",
                newName: "expires");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "refresh_tokens",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "refresh_tokens",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RevokedByIp",
                table: "refresh_tokens",
                newName: "revoked_by_ip");

            migrationBuilder.RenameColumn(
                name: "ReplacedByToken",
                table: "refresh_tokens",
                newName: "replaced_by_token");

            migrationBuilder.RenameColumn(
                name: "ReasonRevoked",
                table: "refresh_tokens",
                newName: "reason_revoked");

            migrationBuilder.RenameColumn(
                name: "CreatedByIp",
                table: "refresh_tokens",
                newName: "created_by_ip");

            migrationBuilder.RenameColumn(
                name: "AccountUserId",
                table: "refresh_tokens",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_AccountUserId",
                table: "refresh_tokens",
                newName: "IX_refresh_tokens_user_id");

            migrationBuilder.AlterColumn<string>(
                name: "password_hash",
                table: "users",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "verified",
                table: "users",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated",
                table: "users",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created",
                table: "users",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "verification_token",
                table: "users",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "system_role",
                table: "users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "reset_token_expires",
                table: "users",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "reset_token",
                table: "users",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "password_reset",
                table: "users",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "accept_terms",
                table: "users",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "token",
                table: "refresh_tokens",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "revoked",
                table: "refresh_tokens",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "expires",
                table: "refresh_tokens",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created",
                table: "refresh_tokens",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "revoked_by_ip",
                table: "refresh_tokens",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "replaced_by_token",
                table: "refresh_tokens",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "reason_revoked",
                table: "refresh_tokens",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "created_by_ip",
                table: "refresh_tokens",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK__users__B9BE370F3C69FB99",
                table: "users",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__refresh___3213E83F1A14E395",
                table: "refresh_tokens",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK__refresh_t__user___6C190EBB",
                table: "refresh_tokens",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__refresh_t__user___6C190EBB",
                table: "refresh_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK__users__B9BE370F3C69FB99",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK__refresh___3213E83F1A14E395",
                table: "refresh_tokens");

            migrationBuilder.RenameTable(
                name: "refresh_tokens",
                newName: "RefreshToken");

            migrationBuilder.RenameColumn(
                name: "verified",
                table: "users",
                newName: "Verified");

            migrationBuilder.RenameColumn(
                name: "updated",
                table: "users",
                newName: "Updated");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "users",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "verification_token",
                table: "users",
                newName: "VerificationToken");

            migrationBuilder.RenameColumn(
                name: "system_role",
                table: "users",
                newName: "SystemRole");

            migrationBuilder.RenameColumn(
                name: "reset_token_expires",
                table: "users",
                newName: "ResetTokenExpires");

            migrationBuilder.RenameColumn(
                name: "reset_token",
                table: "users",
                newName: "ResetToken");

            migrationBuilder.RenameColumn(
                name: "password_reset",
                table: "users",
                newName: "PasswordReset");

            migrationBuilder.RenameColumn(
                name: "accept_terms",
                table: "users",
                newName: "AcceptTerms");

            migrationBuilder.RenameIndex(
                name: "UQ__users__F3DBC57296E81119",
                table: "users",
                newName: "UQ__users__F3DBC572F302E448");

            migrationBuilder.RenameIndex(
                name: "UQ__users__AB6E616418F0B0D9",
                table: "users",
                newName: "UQ__users__AB6E616448A45D4C");

            migrationBuilder.RenameColumn(
                name: "token",
                table: "RefreshToken",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "revoked",
                table: "RefreshToken",
                newName: "Revoked");

            migrationBuilder.RenameColumn(
                name: "expires",
                table: "RefreshToken",
                newName: "Expires");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "RefreshToken",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "RefreshToken",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "revoked_by_ip",
                table: "RefreshToken",
                newName: "RevokedByIp");

            migrationBuilder.RenameColumn(
                name: "replaced_by_token",
                table: "RefreshToken",
                newName: "ReplacedByToken");

            migrationBuilder.RenameColumn(
                name: "reason_revoked",
                table: "RefreshToken",
                newName: "ReasonRevoked");

            migrationBuilder.RenameColumn(
                name: "created_by_ip",
                table: "RefreshToken",
                newName: "CreatedByIp");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "RefreshToken",
                newName: "AccountUserId");

            migrationBuilder.RenameIndex(
                name: "IX_refresh_tokens_user_id",
                table: "RefreshToken",
                newName: "IX_RefreshToken_AccountUserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Verified",
                table: "users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Updated",
                table: "users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "password_hash",
                table: "users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "users",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "VerificationToken",
                table: "users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SystemRole",
                table: "users",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ResetTokenExpires",
                table: "users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ResetToken",
                table: "users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PasswordReset",
                table: "users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AcceptTerms",
                table: "users",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Revoked",
                table: "RefreshToken",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Expires",
                table: "RefreshToken",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "RefreshToken",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "RevokedByIp",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReplacedByToken",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReasonRevoked",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByIp",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK__users__B9BE370F7F35AA08",
                table: "users",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_users_AccountUserId",
                table: "RefreshToken",
                column: "AccountUserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}