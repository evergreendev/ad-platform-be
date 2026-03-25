using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddContacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contact",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AddressLine1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AddressLine2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Zip = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Salutation = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UserRepId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Gender = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LeadSource = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LeadStatus = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    HubspotId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    JobTitle = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Department = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "company_contact",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    StartDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_contact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_company_contact_company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_company_contact_contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contact_email",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DoNotEmail = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contact_email", x => x.Id);
                    table.ForeignKey(
                        name: "FK_contact_email_contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "company_contact_email",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyContactId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DoNotEmail = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_contact_email", x => x.Id);
                    table.ForeignKey(
                        name: "FK_company_contact_email_company_contact_CompanyContactId",
                        column: x => x.CompanyContactId,
                        principalTable: "company_contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "company_contact_phone",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyContactId = table.Column<Guid>(type: "uuid", nullable: false),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DoNotCall = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_contact_phone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_company_contact_phone_company_contact_CompanyContactId",
                        column: x => x.CompanyContactId,
                        principalTable: "company_contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "company_contact_role",
                columns: table => new
                {
                    CompanyContactId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_contact_role", x => new { x.CompanyContactId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_company_contact_role_company_contact_CompanyContactId",
                        column: x => x.CompanyContactId,
                        principalTable: "company_contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_company_contact_role_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_company_contact_CompanyId",
                table: "company_contact",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_company_contact_CompanyId_ContactId",
                table: "company_contact",
                columns: new[] { "CompanyId", "ContactId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_company_contact_CompanyId_IsPrimary",
                table: "company_contact",
                columns: new[] { "CompanyId", "IsPrimary" });

            migrationBuilder.CreateIndex(
                name: "IX_company_contact_ContactId",
                table: "company_contact",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_company_contact_email_CompanyContactId",
                table: "company_contact_email",
                column: "CompanyContactId");

            migrationBuilder.CreateIndex(
                name: "IX_company_contact_email_Email",
                table: "company_contact_email",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_company_contact_phone_CompanyContactId",
                table: "company_contact_phone",
                column: "CompanyContactId");

            migrationBuilder.CreateIndex(
                name: "IX_company_contact_phone_Phone",
                table: "company_contact_phone",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_company_contact_role_RoleId",
                table: "company_contact_role",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_contact_FirstName",
                table: "contact",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_contact_FirstName_LastName",
                table: "contact",
                columns: new[] { "FirstName", "LastName" });

            migrationBuilder.CreateIndex(
                name: "IX_contact_HubspotId",
                table: "contact",
                column: "HubspotId");

            migrationBuilder.CreateIndex(
                name: "IX_contact_IsActive",
                table: "contact",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_contact_LastName",
                table: "contact",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_contact_LastName_FirstName_City",
                table: "contact",
                columns: new[] { "LastName", "FirstName", "City" });

            migrationBuilder.CreateIndex(
                name: "IX_contact_UserRepId",
                table: "contact",
                column: "UserRepId");

            migrationBuilder.CreateIndex(
                name: "IX_contact_email_ContactId",
                table: "contact_email",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_contact_email_Email",
                table: "contact_email",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_role_Name",
                table: "role",
                column: "Name",
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
                name: "company_contact");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "contact");
        }
    }
}
