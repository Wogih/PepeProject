using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    role_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__roles__760965CC0823FF07", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    tag_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tag_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tags__4296A2B6691AE076", x => x.tag_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users__B9BE370FF16AC13E", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "collections",
                columns: table => new
                {
                    collection_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    collection_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__collecti__53D3A5CAA30B7500", x => x.collection_id);
                    table.ForeignKey(
                        name: "FK__collectio__user___3FD07829",
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
                    upload_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__memes__5AD2578F87F22068", x => x.meme_id);
                    table.ForeignKey(
                        name: "FK__memes__user_id__2610A626",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "upload_stats",
                columns: table => new
                {
                    stat_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    upload_count = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    total_views = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    last_upload_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__upload_s__B8A52560473B37ED", x => x.stat_id);
                    table.ForeignKey(
                        name: "FK__upload_st__user___4865BE2A",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__user_rol__6EDEA153CC4A6AC6", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "FK__user_role__role___22401542",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__user_role__user___214BF109",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "collection_memes",
                columns: table => new
                {
                    collection_id = table.Column<int>(type: "int", nullable: false),
                    meme_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__collecti__B67E80B26976637C", x => new { x.collection_id, x.meme_id });
                    table.ForeignKey(
                        name: "FK__collectio__colle__42ACE4D4",
                        column: x => x.collection_id,
                        principalTable: "collections",
                        principalColumn: "collection_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__collectio__meme___43A1090D",
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
                    comment_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__comments__E7957687FA3CF74E", x => x.comment_id);
                    table.ForeignKey(
                        name: "FK__comments__meme_i__3B0BC30C",
                        column: x => x.meme_id,
                        principalTable: "memes",
                        principalColumn: "meme_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__comments__user_i__3BFFE745",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "meme_metadata",
                columns: table => new
                {
                    meme_id = table.Column<int>(type: "int", nullable: false),
                    views = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    shares = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    last_updated = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__meme_met__5AD2578F8F16AD1D", x => x.meme_id);
                    table.ForeignKey(
                        name: "FK__meme_meta__meme___2BC97F7C",
                        column: x => x.meme_id,
                        principalTable: "memes",
                        principalColumn: "meme_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "meme_tags",
                columns: table => new
                {
                    meme_id = table.Column<int>(type: "int", nullable: false),
                    tag_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__meme_tag__2EFB3DA44C7753AF", x => new { x.meme_id, x.tag_id });
                    table.ForeignKey(
                        name: "FK__meme_tags__meme___318258D2",
                        column: x => x.meme_id,
                        principalTable: "memes",
                        principalColumn: "meme_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__meme_tags__tag_i__32767D0B",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "tag_id",
                        onDelete: ReferentialAction.Cascade);
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
                    table.PrimaryKey("PK__reaction__36A9D29813A75BC4", x => x.reaction_id);
                    table.ForeignKey(
                        name: "FK__reactions__meme___36470DEF",
                        column: x => x.meme_id,
                        principalTable: "memes",
                        principalColumn: "meme_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__reactions__user___373B3228",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_collection_memes_meme_id",
                table: "collection_memes",
                column: "meme_id");

            migrationBuilder.CreateIndex(
                name: "IX_collections_user_id",
                table: "collections",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_comments_meme_id",
                table: "comments",
                column: "meme_id");

            migrationBuilder.CreateIndex(
                name: "IX_comments_user_id",
                table: "comments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_meme_tags_tag_id",
                table: "meme_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_memes_user_id",
                table: "memes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_reactions_meme_id",
                table: "reactions",
                column: "meme_id");

            migrationBuilder.CreateIndex(
                name: "IX_reactions_user_id",
                table: "reactions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ__roles__783254B1EC8A87C0",
                table: "roles",
                column: "role_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__tags__E298655CBAEA6588",
                table: "tags",
                column: "tag_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_upload_stats_user_id",
                table: "upload_stats",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_role_id",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "UQ__users__AB6E61643B91F4FA",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__users__F3DBC572F86B8864",
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
