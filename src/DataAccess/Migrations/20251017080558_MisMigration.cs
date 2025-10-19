using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MisMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__collectio__colle__42ACE4D4",
                table: "collection_memes");

            migrationBuilder.DropForeignKey(
                name: "FK__collectio__meme___43A1090D",
                table: "collection_memes");

            migrationBuilder.DropForeignKey(
                name: "FK__collectio__user___3FD07829",
                table: "collections");

            migrationBuilder.DropForeignKey(
                name: "FK__comments__meme_i__3B0BC30C",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK__comments__user_i__3BFFE745",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK__meme_meta__meme___2BC97F7C",
                table: "meme_metadata");

            migrationBuilder.DropForeignKey(
                name: "FK__meme_tags__meme___318258D2",
                table: "meme_tags");

            migrationBuilder.DropForeignKey(
                name: "FK__meme_tags__tag_i__32767D0B",
                table: "meme_tags");

            migrationBuilder.DropForeignKey(
                name: "FK__memes__user_id__2610A626",
                table: "memes");

            migrationBuilder.DropForeignKey(
                name: "FK__reactions__meme___36470DEF",
                table: "reactions");

            migrationBuilder.DropForeignKey(
                name: "FK__reactions__user___373B3228",
                table: "reactions");

            migrationBuilder.DropForeignKey(
                name: "FK__upload_st__user___4865BE2A",
                table: "upload_stats");

            migrationBuilder.DropForeignKey(
                name: "FK__user_role__role___22401542",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK__user_role__user___214BF109",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK__users__B9BE370FF16AC13E",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK__user_rol__6EDEA153CC4A6AC6",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK__upload_s__B8A52560473B37ED",
                table: "upload_stats");

            migrationBuilder.DropIndex(
                name: "IX_upload_stats_user_id",
                table: "upload_stats");

            migrationBuilder.DropPrimaryKey(
                name: "PK__tags__4296A2B6691AE076",
                table: "tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK__roles__760965CC0823FF07",
                table: "roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK__reaction__36A9D29813A75BC4",
                table: "reactions");

            migrationBuilder.DropIndex(
                name: "IX_reactions_user_id",
                table: "reactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK__memes__5AD2578F87F22068",
                table: "memes");

            migrationBuilder.DropPrimaryKey(
                name: "PK__meme_tag__2EFB3DA44C7753AF",
                table: "meme_tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK__meme_met__5AD2578F8F16AD1D",
                table: "meme_metadata");

            migrationBuilder.DropPrimaryKey(
                name: "PK__comments__E7957687FA3CF74E",
                table: "comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK__collecti__53D3A5CAA30B7500",
                table: "collections");

            migrationBuilder.DropIndex(
                name: "IX_collections_user_id",
                table: "collections");

            migrationBuilder.DropPrimaryKey(
                name: "PK__collecti__B67E80B26976637C",
                table: "collection_memes");

            migrationBuilder.DropColumn(
                name: "shares",
                table: "meme_metadata");

            migrationBuilder.DropColumn(
                name: "views",
                table: "meme_metadata");

            migrationBuilder.RenameIndex(
                name: "UQ__users__F3DBC572F86B8864",
                table: "users",
                newName: "UQ__users__F3DBC572F302E448");

            migrationBuilder.RenameIndex(
                name: "UQ__users__AB6E61643B91F4FA",
                table: "users",
                newName: "UQ__users__AB6E616448A45D4C");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "upload_stats",
                newName: "meme_id");

            migrationBuilder.RenameColumn(
                name: "upload_count",
                table: "upload_stats",
                newName: "views_count");

            migrationBuilder.RenameColumn(
                name: "total_views",
                table: "upload_stats",
                newName: "share_count");

            migrationBuilder.RenameColumn(
                name: "last_upload_date",
                table: "upload_stats",
                newName: "last_viewed");

            migrationBuilder.RenameIndex(
                name: "UQ__tags__E298655CBAEA6588",
                table: "tags",
                newName: "UQ__tags__E298655CA7AA5A3E");

            migrationBuilder.RenameIndex(
                name: "UQ__roles__783254B1EC8A87C0",
                table: "roles",
                newName: "UQ__roles__783254B187997332");

            migrationBuilder.RenameColumn(
                name: "last_updated",
                table: "meme_metadata",
                newName: "created_at");

            migrationBuilder.AddColumn<int>(
                name: "user_role_id",
                table: "user_roles",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "assigned_at",
                table: "user_roles",
                type: "datetime",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<int>(
                name: "download_count",
                table: "upload_stats",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_updated",
                table: "upload_stats",
                type: "datetime",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "tags",
                type: "datetime",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "roles",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_public",
                table: "memes",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "meme_tag_id",
                table: "meme_tags",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "tagged_at",
                table: "meme_tags",
                type: "datetime",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<int>(
                name: "metadata_id",
                table: "meme_metadata",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "file_format",
                table: "meme_metadata",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "file_size",
                table: "meme_metadata",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "height",
                table: "meme_metadata",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "mime_type",
                table: "meme_metadata",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "width",
                table: "meme_metadata",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "edited_at",
                table: "comments",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_edited",
                table: "comments",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "parent_comment_id",
                table: "comments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "collections",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_public",
                table: "collections",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "collections",
                type: "datetime",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<int>(
                name: "collection_meme_id",
                table: "collection_memes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "added_at",
                table: "collection_memes",
                type: "datetime",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddPrimaryKey(
                name: "PK__users__B9BE370F7F35AA08",
                table: "users",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__user_rol__B8D9ABA2CDA4630A",
                table: "user_roles",
                column: "user_role_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__upload_s__B8A52560A415E415",
                table: "upload_stats",
                column: "stat_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__tags__4296A2B6C2048044",
                table: "tags",
                column: "tag_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__roles__760965CCD6CF1D64",
                table: "roles",
                column: "role_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__reaction__36A9D29816879338",
                table: "reactions",
                column: "reaction_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__memes__5AD2578F5B5FFD53",
                table: "memes",
                column: "meme_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__meme_tag__16773B431689E4B4",
                table: "meme_tags",
                column: "meme_tag_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__meme_met__C1088FC4BB567167",
                table: "meme_metadata",
                column: "metadata_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__comments__E79576870956273D",
                table: "comments",
                column: "comment_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__collecti__53D3A5CAB5272981",
                table: "collections",
                column: "collection_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__collecti__019B66EEB1148A19",
                table: "collection_memes",
                column: "collection_meme_id");

            migrationBuilder.CreateIndex(
                name: "UC_UserRole",
                table: "user_roles",
                columns: new[] { "user_id", "role_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__upload_s__5AD2578EADFA737A",
                table: "upload_stats",
                column: "meme_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UC_UserMemeReaction",
                table: "reactions",
                columns: new[] { "user_id", "meme_id", "reaction_type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UC_MemeTag",
                table: "meme_tags",
                columns: new[] { "meme_id", "tag_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__meme_met__5AD2578E3117FEA1",
                table: "meme_metadata",
                column: "meme_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_comments_parent_comment_id",
                table: "comments",
                column: "parent_comment_id");

            migrationBuilder.CreateIndex(
                name: "UC_UserCollectionName",
                table: "collections",
                columns: new[] { "user_id", "collection_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UC_CollectionMeme",
                table: "collection_memes",
                columns: new[] { "collection_id", "meme_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK__collectio__colle__6E01572D",
                table: "collection_memes",
                column: "collection_id",
                principalTable: "collections",
                principalColumn: "collection_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__collectio__meme___6EF57B66",
                table: "collection_memes",
                column: "meme_id",
                principalTable: "memes",
                principalColumn: "meme_id");

            migrationBuilder.AddForeignKey(
                name: "FK__collectio__user___693CA210",
                table: "collections",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__comments__meme_i__60A75C0F",
                table: "comments",
                column: "meme_id",
                principalTable: "memes",
                principalColumn: "meme_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__comments__parent__628FA481",
                table: "comments",
                column: "parent_comment_id",
                principalTable: "comments",
                principalColumn: "comment_id");

            migrationBuilder.AddForeignKey(
                name: "FK__comments__user_i__619B8048",
                table: "comments",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK__meme_meta__meme___4BAC3F29",
                table: "meme_metadata",
                column: "meme_id",
                principalTable: "memes",
                principalColumn: "meme_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__meme_tags__meme___5441852A",
                table: "meme_tags",
                column: "meme_id",
                principalTable: "memes",
                principalColumn: "meme_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__meme_tags__tag_i__5535A963",
                table: "meme_tags",
                column: "tag_id",
                principalTable: "tags",
                principalColumn: "tag_id");

            migrationBuilder.AddForeignKey(
                name: "FK__memes__user_id__46E78A0C",
                table: "memes",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__reactions__meme___5AEE82B9",
                table: "reactions",
                column: "meme_id",
                principalTable: "memes",
                principalColumn: "meme_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__reactions__user___5BE2A6F2",
                table: "reactions",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK__upload_st__meme___76969D2E",
                table: "upload_stats",
                column: "meme_id",
                principalTable: "memes",
                principalColumn: "meme_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__user_role__role___4222D4EF",
                table: "user_roles",
                column: "role_id",
                principalTable: "roles",
                principalColumn: "role_id");

            migrationBuilder.AddForeignKey(
                name: "FK__user_role__user___412EB0B6",
                table: "user_roles",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__collectio__colle__6E01572D",
                table: "collection_memes");

            migrationBuilder.DropForeignKey(
                name: "FK__collectio__meme___6EF57B66",
                table: "collection_memes");

            migrationBuilder.DropForeignKey(
                name: "FK__collectio__user___693CA210",
                table: "collections");

            migrationBuilder.DropForeignKey(
                name: "FK__comments__meme_i__60A75C0F",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK__comments__parent__628FA481",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK__comments__user_i__619B8048",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK__meme_meta__meme___4BAC3F29",
                table: "meme_metadata");

            migrationBuilder.DropForeignKey(
                name: "FK__meme_tags__meme___5441852A",
                table: "meme_tags");

            migrationBuilder.DropForeignKey(
                name: "FK__meme_tags__tag_i__5535A963",
                table: "meme_tags");

            migrationBuilder.DropForeignKey(
                name: "FK__memes__user_id__46E78A0C",
                table: "memes");

            migrationBuilder.DropForeignKey(
                name: "FK__reactions__meme___5AEE82B9",
                table: "reactions");

            migrationBuilder.DropForeignKey(
                name: "FK__reactions__user___5BE2A6F2",
                table: "reactions");

            migrationBuilder.DropForeignKey(
                name: "FK__upload_st__meme___76969D2E",
                table: "upload_stats");

            migrationBuilder.DropForeignKey(
                name: "FK__user_role__role___4222D4EF",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK__user_role__user___412EB0B6",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK__users__B9BE370F7F35AA08",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK__user_rol__B8D9ABA2CDA4630A",
                table: "user_roles");

            migrationBuilder.DropIndex(
                name: "UC_UserRole",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK__upload_s__B8A52560A415E415",
                table: "upload_stats");

            migrationBuilder.DropIndex(
                name: "UQ__upload_s__5AD2578EADFA737A",
                table: "upload_stats");

            migrationBuilder.DropPrimaryKey(
                name: "PK__tags__4296A2B6C2048044",
                table: "tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK__roles__760965CCD6CF1D64",
                table: "roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK__reaction__36A9D29816879338",
                table: "reactions");

            migrationBuilder.DropIndex(
                name: "UC_UserMemeReaction",
                table: "reactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK__memes__5AD2578F5B5FFD53",
                table: "memes");

            migrationBuilder.DropPrimaryKey(
                name: "PK__meme_tag__16773B431689E4B4",
                table: "meme_tags");

            migrationBuilder.DropIndex(
                name: "UC_MemeTag",
                table: "meme_tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK__meme_met__C1088FC4BB567167",
                table: "meme_metadata");

            migrationBuilder.DropIndex(
                name: "UQ__meme_met__5AD2578E3117FEA1",
                table: "meme_metadata");

            migrationBuilder.DropPrimaryKey(
                name: "PK__comments__E79576870956273D",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "IX_comments_parent_comment_id",
                table: "comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK__collecti__53D3A5CAB5272981",
                table: "collections");

            migrationBuilder.DropIndex(
                name: "UC_UserCollectionName",
                table: "collections");

            migrationBuilder.DropPrimaryKey(
                name: "PK__collecti__019B66EEB1148A19",
                table: "collection_memes");

            migrationBuilder.DropIndex(
                name: "UC_CollectionMeme",
                table: "collection_memes");

            migrationBuilder.DropColumn(
                name: "user_role_id",
                table: "user_roles");

            migrationBuilder.DropColumn(
                name: "assigned_at",
                table: "user_roles");

            migrationBuilder.DropColumn(
                name: "download_count",
                table: "upload_stats");

            migrationBuilder.DropColumn(
                name: "last_updated",
                table: "upload_stats");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "description",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "is_public",
                table: "memes");

            migrationBuilder.DropColumn(
                name: "meme_tag_id",
                table: "meme_tags");

            migrationBuilder.DropColumn(
                name: "tagged_at",
                table: "meme_tags");

            migrationBuilder.DropColumn(
                name: "metadata_id",
                table: "meme_metadata");

            migrationBuilder.DropColumn(
                name: "file_format",
                table: "meme_metadata");

            migrationBuilder.DropColumn(
                name: "file_size",
                table: "meme_metadata");

            migrationBuilder.DropColumn(
                name: "height",
                table: "meme_metadata");

            migrationBuilder.DropColumn(
                name: "mime_type",
                table: "meme_metadata");

            migrationBuilder.DropColumn(
                name: "width",
                table: "meme_metadata");

            migrationBuilder.DropColumn(
                name: "edited_at",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "is_edited",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "parent_comment_id",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "description",
                table: "collections");

            migrationBuilder.DropColumn(
                name: "is_public",
                table: "collections");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "collections");

            migrationBuilder.DropColumn(
                name: "collection_meme_id",
                table: "collection_memes");

            migrationBuilder.DropColumn(
                name: "added_at",
                table: "collection_memes");

            migrationBuilder.RenameIndex(
                name: "UQ__users__F3DBC572F302E448",
                table: "users",
                newName: "UQ__users__F3DBC572F86B8864");

            migrationBuilder.RenameIndex(
                name: "UQ__users__AB6E616448A45D4C",
                table: "users",
                newName: "UQ__users__AB6E61643B91F4FA");

            migrationBuilder.RenameColumn(
                name: "views_count",
                table: "upload_stats",
                newName: "upload_count");

            migrationBuilder.RenameColumn(
                name: "share_count",
                table: "upload_stats",
                newName: "total_views");

            migrationBuilder.RenameColumn(
                name: "meme_id",
                table: "upload_stats",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "last_viewed",
                table: "upload_stats",
                newName: "last_upload_date");

            migrationBuilder.RenameIndex(
                name: "UQ__tags__E298655CA7AA5A3E",
                table: "tags",
                newName: "UQ__tags__E298655CBAEA6588");

            migrationBuilder.RenameIndex(
                name: "UQ__roles__783254B187997332",
                table: "roles",
                newName: "UQ__roles__783254B1EC8A87C0");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "meme_metadata",
                newName: "last_updated");

            migrationBuilder.AddColumn<int>(
                name: "shares",
                table: "meme_metadata",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "views",
                table: "meme_metadata",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK__users__B9BE370FF16AC13E",
                table: "users",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__user_rol__6EDEA153CC4A6AC6",
                table: "user_roles",
                columns: new[] { "user_id", "role_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK__upload_s__B8A52560473B37ED",
                table: "upload_stats",
                column: "stat_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__tags__4296A2B6691AE076",
                table: "tags",
                column: "tag_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__roles__760965CC0823FF07",
                table: "roles",
                column: "role_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__reaction__36A9D29813A75BC4",
                table: "reactions",
                column: "reaction_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__memes__5AD2578F87F22068",
                table: "memes",
                column: "meme_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__meme_tag__2EFB3DA44C7753AF",
                table: "meme_tags",
                columns: new[] { "meme_id", "tag_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK__meme_met__5AD2578F8F16AD1D",
                table: "meme_metadata",
                column: "meme_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__comments__E7957687FA3CF74E",
                table: "comments",
                column: "comment_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__collecti__53D3A5CAA30B7500",
                table: "collections",
                column: "collection_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__collecti__B67E80B26976637C",
                table: "collection_memes",
                columns: new[] { "collection_id", "meme_id" });

            migrationBuilder.CreateIndex(
                name: "IX_upload_stats_user_id",
                table: "upload_stats",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_reactions_user_id",
                table: "reactions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_collections_user_id",
                table: "collections",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK__collectio__colle__42ACE4D4",
                table: "collection_memes",
                column: "collection_id",
                principalTable: "collections",
                principalColumn: "collection_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__collectio__meme___43A1090D",
                table: "collection_memes",
                column: "meme_id",
                principalTable: "memes",
                principalColumn: "meme_id");

            migrationBuilder.AddForeignKey(
                name: "FK__collectio__user___3FD07829",
                table: "collections",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__comments__meme_i__3B0BC30C",
                table: "comments",
                column: "meme_id",
                principalTable: "memes",
                principalColumn: "meme_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__comments__user_i__3BFFE745",
                table: "comments",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK__meme_meta__meme___2BC97F7C",
                table: "meme_metadata",
                column: "meme_id",
                principalTable: "memes",
                principalColumn: "meme_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__meme_tags__meme___318258D2",
                table: "meme_tags",
                column: "meme_id",
                principalTable: "memes",
                principalColumn: "meme_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__meme_tags__tag_i__32767D0B",
                table: "meme_tags",
                column: "tag_id",
                principalTable: "tags",
                principalColumn: "tag_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__memes__user_id__2610A626",
                table: "memes",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__reactions__meme___36470DEF",
                table: "reactions",
                column: "meme_id",
                principalTable: "memes",
                principalColumn: "meme_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__reactions__user___373B3228",
                table: "reactions",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK__upload_st__user___4865BE2A",
                table: "upload_stats",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__user_role__role___22401542",
                table: "user_roles",
                column: "role_id",
                principalTable: "roles",
                principalColumn: "role_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__user_role__user___214BF109",
                table: "user_roles",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
