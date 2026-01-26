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
            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__roles__760965CCD6CF1D64", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    tag_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tag_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tags__4296A2B6C2048044", x => x.tag_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    accept_terms = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    system_role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    verification_token = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    verified = table.Column<DateTime>(type: "datetime", nullable: true),
                    reset_token = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    reset_token_expires = table.Column<DateTime>(type: "datetime", nullable: true),
                    password_reset = table.Column<DateTime>(type: "datetime", nullable: true),
                    created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updated = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users__B9BE370F3C69FB99", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "collections",
                columns: table => new
                {
                    collection_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    collection_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    is_public = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__collecti__53D3A5CAB5272981", x => x.collection_id);
                    table.ForeignKey(
                        name: "FK__collectio__user___693CA210",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "memes",
                columns: table => new
                {
                    meme_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    image_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    upload_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    is_public = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__memes__5AD2578F5B5FFD53", x => x.meme_id);
                    table.ForeignKey(
                        name: "FK__memes__user_id__46E78A0C",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    token = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    expires = table.Column<DateTime>(type: "datetime", nullable: false),
                    created = table.Column<DateTime>(type: "datetime", nullable: false),
                    created_by_ip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    revoked = table.Column<DateTime>(type: "datetime", nullable: true),
                    revoked_by_ip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    replaced_by_token = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    reason_revoked = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__refresh___3213E83F1A14E395", x => x.id);
                    table.ForeignKey(
                        name: "FK__refresh_t__user___6C190EBB",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    user_role_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    assigned_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__user_rol__B8D9ABA2CDA4630A", x => x.user_role_id);
                    table.ForeignKey(
                        name: "FK__user_role__role___4222D4EF",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "role_id");
                    table.ForeignKey(
                        name: "FK__user_role__user___412EB0B6",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "collection_memes",
                columns: table => new
                {
                    collection_meme_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    collection_id = table.Column<int>(type: "int", nullable: false),
                    meme_id = table.Column<int>(type: "int", nullable: false),
                    added_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__collecti__019B66EEB1148A19", x => x.collection_meme_id);
                    table.ForeignKey(
                        name: "FK__collectio__colle__6E01572D",
                        column: x => x.collection_id,
                        principalTable: "collections",
                        principalColumn: "collection_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__collectio__meme___6EF57B66",
                        column: x => x.meme_id,
                        principalTable: "memes",
                        principalColumn: "meme_id");
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    comment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    meme_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    comment_text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    comment_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    parent_comment_id = table.Column<int>(type: "int", nullable: true),
                    is_edited = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    edited_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__comments__E79576870956273D", x => x.comment_id);
                    table.ForeignKey(
                        name: "FK__comments__meme_i__60A75C0F",
                        column: x => x.meme_id,
                        principalTable: "memes",
                        principalColumn: "meme_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__comments__parent__628FA481",
                        column: x => x.parent_comment_id,
                        principalTable: "comments",
                        principalColumn: "comment_id");
                    table.ForeignKey(
                        name: "FK__comments__user_i__619B8048",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "meme_metadata",
                columns: table => new
                {
                    metadata_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    meme_id = table.Column<int>(type: "int", nullable: false),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    width = table.Column<int>(type: "int", nullable: false),
                    height = table.Column<int>(type: "int", nullable: false),
                    file_format = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    mime_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__meme_met__C1088FC4BB567167", x => x.metadata_id);
                    table.ForeignKey(
                        name: "FK__meme_meta__meme___4BAC3F29",
                        column: x => x.meme_id,
                        principalTable: "memes",
                        principalColumn: "meme_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "meme_tags",
                columns: table => new
                {
                    meme_tag_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    meme_id = table.Column<int>(type: "int", nullable: false),
                    tag_id = table.Column<int>(type: "int", nullable: false),
                    tagged_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__meme_tag__16773B431689E4B4", x => x.meme_tag_id);
                    table.ForeignKey(
                        name: "FK__meme_tags__meme___5441852A",
                        column: x => x.meme_id,
                        principalTable: "memes",
                        principalColumn: "meme_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__meme_tags__tag_i__5535A963",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "tag_id");
                });

            migrationBuilder.CreateTable(
                name: "reactions",
                columns: table => new
                {
                    reaction_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    meme_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    reaction_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    reaction_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__reaction__36A9D29816879338", x => x.reaction_id);
                    table.ForeignKey(
                        name: "FK__reactions__meme___5AEE82B9",
                        column: x => x.meme_id,
                        principalTable: "memes",
                        principalColumn: "meme_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__reactions__user___5BE2A6F2",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "upload_stats",
                columns: table => new
                {
                    stat_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    meme_id = table.Column<int>(type: "int", nullable: false),
                    views_count = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    download_count = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    share_count = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    last_viewed = table.Column<DateTime>(type: "datetime", nullable: true),
                    last_updated = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__upload_s__B8A52560A415E415", x => x.stat_id);
                    table.ForeignKey(
                        name: "FK__upload_st__meme___76969D2E",
                        column: x => x.meme_id,
                        principalTable: "memes",
                        principalColumn: "meme_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_collection_memes_meme_id",
                table: "collection_memes",
                column: "meme_id");

            migrationBuilder.CreateIndex(
                name: "UC_CollectionMeme",
                table: "collection_memes",
                columns: new[] { "collection_id", "meme_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UC_UserCollectionName",
                table: "collections",
                columns: new[] { "user_id", "collection_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_comments_meme_id",
                table: "comments",
                column: "meme_id");

            migrationBuilder.CreateIndex(
                name: "IX_comments_parent_comment_id",
                table: "comments",
                column: "parent_comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_comments_user_id",
                table: "comments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ__meme_met__5AD2578E3117FEA1",
                table: "meme_metadata",
                column: "meme_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_meme_tags_tag_id",
                table: "meme_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "UC_MemeTag",
                table: "meme_tags",
                columns: new[] { "meme_id", "tag_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_memes_user_id",
                table: "memes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_reactions_meme_id",
                table: "reactions",
                column: "meme_id");

            migrationBuilder.CreateIndex(
                name: "UC_UserMemeReaction",
                table: "reactions",
                columns: new[] { "user_id", "meme_id", "reaction_type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ__roles__783254B187997332",
                table: "roles",
                column: "role_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__tags__E298655CA7AA5A3E",
                table: "tags",
                column: "tag_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__upload_s__5AD2578EADFA737A",
                table: "upload_stats",
                column: "meme_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_role_id",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "UC_UserRole",
                table: "user_roles",
                columns: new[] { "user_id", "role_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__users__AB6E616418F0B0D9",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__users__F3DBC57296E81119",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "collection_memes");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "meme_metadata");

            migrationBuilder.DropTable(
                name: "meme_tags");

            migrationBuilder.DropTable(
                name: "reactions");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "upload_stats");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "collections");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "memes");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}