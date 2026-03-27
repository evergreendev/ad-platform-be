using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialAPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "company",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    address2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    state = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    zip = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    website_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    tax_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    last_update = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    collections = table.Column<bool>(type: "boolean", nullable: false),
                    write_off = table.Column<bool>(type: "boolean", nullable: false),
                    primary_rep_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    legacy_primary_category = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    hubspot_company_id = table.Column<long>(type: "bigint", nullable: true),
                    latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    longitude = table.Column<decimal>(type: "numeric", nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    dead = table.Column<bool>(type: "boolean", nullable: false),
                    is_new_company = table.Column<bool>(type: "boolean", nullable: false),
                    company_special_billing = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "contact",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    address_line1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    address_line2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    state = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    zip = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    salutation = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    user_rep_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    gender = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    lead_source = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    lead_status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    hubspot_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    job_title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    department = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp", nullable: true),
                    last_updated_date = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contact", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "external_id_map",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    source_system = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    source_table = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    source_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    entity_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    internal_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_external_id_map", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_system = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "company_contact",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false),
                    contact_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    start_date = table.Column<DateTime>(type: "timestamp", nullable: true),
                    end_date = table.Column<DateTime>(type: "timestamp", nullable: true),
                    notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company_contact", x => x.id);
                    table.ForeignKey(
                        name: "fk_company_contact_company_company_id",
                        column: x => x.company_id,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_company_contact_contact_contact_id",
                        column: x => x.contact_id,
                        principalTable: "contact",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contact_email",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    contact_id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    do_not_email = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contact_email", x => x.id);
                    table.ForeignKey(
                        name: "fk_contact_email_contact_contact_id",
                        column: x => x.contact_id,
                        principalTable: "contact",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "company_contact_email",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_contact_id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    do_not_email = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company_contact_email", x => x.id);
                    table.ForeignKey(
                        name: "fk_company_contact_email_company_contact_company_contact_id",
                        column: x => x.company_contact_id,
                        principalTable: "company_contact",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "company_contact_phone",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_contact_id = table.Column<Guid>(type: "uuid", nullable: false),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    do_not_call = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company_contact_phone", x => x.id);
                    table.ForeignKey(
                        name: "fk_company_contact_phone_company_contact_company_contact_id",
                        column: x => x.company_contact_id,
                        principalTable: "company_contact",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "company_contact_role",
                columns: table => new
                {
                    company_contact_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company_contact_role", x => new { x.company_contact_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_company_contact_role_company_contact_company_contact_id",
                        column: x => x.company_contact_id,
                        principalTable: "company_contact",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_company_contact_role_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "id", "is_system", "name" },
                values: new object[,]
                {
                    { 1, true, "Billing" },
                    { 2, true, "Print Artwork" },
                    { 3, true, "Digital Artwork" },
                    { 4, true, "Marketing" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_company_contact_company_id",
                table: "company_contact",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_contact_company_id_contact_id",
                table: "company_contact",
                columns: new[] { "company_id", "contact_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_company_contact_company_id_is_primary",
                table: "company_contact",
                columns: new[] { "company_id", "is_primary" });

            migrationBuilder.CreateIndex(
                name: "ix_company_contact_contact_id",
                table: "company_contact",
                column: "contact_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_contact_email_company_contact_id",
                table: "company_contact_email",
                column: "company_contact_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_contact_email_email",
                table: "company_contact_email",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "ix_company_contact_phone_company_contact_id",
                table: "company_contact_phone",
                column: "company_contact_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_contact_phone_phone",
                table: "company_contact_phone",
                column: "phone");

            migrationBuilder.CreateIndex(
                name: "ix_company_contact_role_role_id",
                table: "company_contact_role",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_contact_first_name",
                table: "contact",
                column: "first_name");

            migrationBuilder.CreateIndex(
                name: "ix_contact_first_name_last_name",
                table: "contact",
                columns: new[] { "first_name", "last_name" });

            migrationBuilder.CreateIndex(
                name: "ix_contact_hubspot_id",
                table: "contact",
                column: "hubspot_id");

            migrationBuilder.CreateIndex(
                name: "ix_contact_is_active",
                table: "contact",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_contact_last_name",
                table: "contact",
                column: "last_name");

            migrationBuilder.CreateIndex(
                name: "ix_contact_last_name_first_name_city",
                table: "contact",
                columns: new[] { "last_name", "first_name", "city" });

            migrationBuilder.CreateIndex(
                name: "ix_contact_user_rep_id",
                table: "contact",
                column: "user_rep_id");

            migrationBuilder.CreateIndex(
                name: "ix_contact_email_contact_id",
                table: "contact_email",
                column: "contact_id");

            migrationBuilder.CreateIndex(
                name: "ix_contact_email_email",
                table: "contact_email",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "ix_external_id_map_source_system_source_table_source_id",
                table: "external_id_map",
                columns: new[] { "source_system", "source_table", "source_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_role_name",
                table: "role",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "company_contact_email");

            migrationBuilder.DropTable(
                name: "company_contact_phone");

            migrationBuilder.DropTable(
                name: "company_contact_role");

            migrationBuilder.DropTable(
                name: "contact_email");

            migrationBuilder.DropTable(
                name: "external_id_map");

            migrationBuilder.DropTable(
                name: "company_contact");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "company");

            migrationBuilder.DropTable(
                name: "contact");
        }
    }
}
