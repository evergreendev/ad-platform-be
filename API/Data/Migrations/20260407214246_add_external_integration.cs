using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_external_integration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "integration_connection",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    display_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    auth_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    metadata_json = table.Column<string>(type: "jsonb", nullable: true),
                    last_synced_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_integration_connection", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "external_record_link",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    integration_connection_id = table.Column<Guid>(type: "uuid", nullable: false),
                    internal_entity_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    internal_entity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    external_entity_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    external_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    sync_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    last_synced_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    last_sync_error = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    provider_metadata_json = table.Column<string>(type: "jsonb", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_external_record_link", x => x.id);
                    table.ForeignKey(
                        name: "fk_external_record_link_integration_connection_integration_con",
                        column: x => x.integration_connection_id,
                        principalTable: "integration_connection",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "integration_sync_log",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    integration_connection_id = table.Column<Guid>(type: "uuid", nullable: false),
                    external_record_link_id = table.Column<Guid>(type: "uuid", nullable: true),
                    direction = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    message = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    payload_json = table.Column<string>(type: "jsonb", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_integration_sync_log", x => x.id);
                    table.ForeignKey(
                        name: "fk_integration_sync_log_external_record_link_external_record_l",
                        column: x => x.external_record_link_id,
                        principalTable: "external_record_link",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_integration_sync_log_integration_connection_integration_con",
                        column: x => x.integration_connection_id,
                        principalTable: "integration_connection",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_external_record_link_integration_connection_id_external_ent",
                table: "external_record_link",
                columns: new[] { "integration_connection_id", "external_entity_type", "external_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_external_record_link_integration_connection_id_internal_ent",
                table: "external_record_link",
                columns: new[] { "integration_connection_id", "internal_entity_type", "internal_entity_id", "external_entity_type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_integration_connection_provider_display_name",
                table: "integration_connection",
                columns: new[] { "provider", "display_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_integration_sync_log_created_at",
                table: "integration_sync_log",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_integration_sync_log_external_record_link_id",
                table: "integration_sync_log",
                column: "external_record_link_id");

            migrationBuilder.CreateIndex(
                name: "ix_integration_sync_log_integration_connection_id",
                table: "integration_sync_log",
                column: "integration_connection_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "integration_sync_log");

            migrationBuilder.DropTable(
                name: "external_record_link");

            migrationBuilder.DropTable(
                name: "integration_connection");
        }
    }
}
