using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMailMergeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mail_merge_template",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    subject_template = table.Column<string>(type: "text", nullable: false),
                    body_json = table.Column<string>(type: "jsonb", nullable: false),
                    body_html = table.Column<string>(type: "text", nullable: true),
                    plain_text = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mail_merge_template", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "merge_field",
                columns: table => new
                {
                    key = table.Column<string>(type: "text", nullable: false),
                    label = table.Column<string>(type: "text", nullable: false),
                    entity = table.Column<string>(type: "text", nullable: false),
                    data_type = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_merge_field", x => x.key);
                });

            migrationBuilder.CreateTable(
                name: "mail_merge_template_field",
                columns: table => new
                {
                    template_id = table.Column<Guid>(type: "uuid", nullable: false),
                    merge_field_key = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mail_merge_template_field", x => new { x.template_id, x.merge_field_key });
                    table.ForeignKey(
                        name: "fk_mail_merge_template_field_mail_merge_template_template_id",
                        column: x => x.template_id,
                        principalTable: "mail_merge_template",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_mail_merge_template_field_merge_field_merge_field_key",
                        column: x => x.merge_field_key,
                        principalTable: "merge_field",
                        principalColumn: "key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_mail_merge_template_field_merge_field_key",
                table: "mail_merge_template_field",
                column: "merge_field_key");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mail_merge_template_field");

            migrationBuilder.DropTable(
                name: "mail_merge_template");

            migrationBuilder.DropTable(
                name: "merge_field");
        }
    }
}
