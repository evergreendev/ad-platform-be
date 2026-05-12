using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailMessageEventAndLinkTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "email_message",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    campaign_id = table.Column<Guid>(type: "uuid", nullable: true),
                    contact_id = table.Column<Guid>(type: "uuid", nullable: true),
                    company_contact_id = table.Column<Guid>(type: "uuid", nullable: true),
                    mail_merge_template_id = table.Column<Guid>(type: "uuid", nullable: true),
                    to_email_address = table.Column<string>(type: "text", nullable: false),
                    to_display_name = table.Column<string>(type: "text", nullable: true),
                    subject = table.Column<string>(type: "text", nullable: false),
                    body_html = table.Column<string>(type: "text", nullable: false),
                    plain_text = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    provider = table.Column<string>(type: "text", nullable: true),
                    provider_message_id = table.Column<string>(type: "text", nullable: true),
                    queued_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    sent_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    delivered_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_email_message", x => x.id);
                    table.ForeignKey(
                        name: "fk_email_message_campaigns_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "campaigns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_email_message_company_contact_company_contact_id",
                        column: x => x.company_contact_id,
                        principalTable: "company_contact",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_email_message_contact_contact_id",
                        column: x => x.contact_id,
                        principalTable: "contact",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_email_message_mail_merge_template_mail_merge_template_id",
                        column: x => x.mail_merge_template_id,
                        principalTable: "mail_merge_template",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "email_event",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email_message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_type = table.Column<string>(type: "text", nullable: false),
                    occurred_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    received_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    provider_event_id = table.Column<string>(type: "text", nullable: false),
                    metadata_json = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_email_event", x => x.id);
                    table.ForeignKey(
                        name: "fk_email_event_email_message_email_message_id",
                        column: x => x.email_message_id,
                        principalTable: "email_message",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "email_link",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email_message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    original_url = table.Column<string>(type: "text", nullable: false),
                    label = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_email_link", x => x.id);
                    table.ForeignKey(
                        name: "fk_email_link_email_message_email_message_id",
                        column: x => x.email_message_id,
                        principalTable: "email_message",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_email_event_email_message_id",
                table: "email_event",
                column: "email_message_id");

            migrationBuilder.CreateIndex(
                name: "ix_email_event_event_type",
                table: "email_event",
                column: "event_type");

            migrationBuilder.CreateIndex(
                name: "ix_email_event_occurred_at",
                table: "email_event",
                column: "occurred_at");

            migrationBuilder.CreateIndex(
                name: "ix_email_event_provider_event_id",
                table: "email_event",
                column: "provider_event_id");

            migrationBuilder.CreateIndex(
                name: "ix_email_link_email_message_id",
                table: "email_link",
                column: "email_message_id");

            migrationBuilder.CreateIndex(
                name: "ix_email_message_campaign_id",
                table: "email_message",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "ix_email_message_company_contact_id",
                table: "email_message",
                column: "company_contact_id");

            migrationBuilder.CreateIndex(
                name: "ix_email_message_contact_id",
                table: "email_message",
                column: "contact_id");

            migrationBuilder.CreateIndex(
                name: "ix_email_message_mail_merge_template_id",
                table: "email_message",
                column: "mail_merge_template_id");

            migrationBuilder.CreateIndex(
                name: "ix_email_message_provider_message_id",
                table: "email_message",
                column: "provider_message_id");

            migrationBuilder.CreateIndex(
                name: "ix_email_message_status",
                table: "email_message",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "email_event");

            migrationBuilder.DropTable(
                name: "email_link");

            migrationBuilder.DropTable(
                name: "email_message");
        }
    }
}
