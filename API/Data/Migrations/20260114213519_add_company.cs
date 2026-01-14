using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_company : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "company",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Address2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Zip = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    WebsiteUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TaxId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    LastUpdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Collections = table.Column<bool>(type: "boolean", nullable: false),
                    WriteOff = table.Column<bool>(type: "boolean", nullable: false),
                    PrimaryRepName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AssociatedCompany = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LegacyBillingContact = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LegacyPrimaryContact = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LegacyPrimaryCategory = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    HubspotCompanyId = table.Column<long>(type: "bigint", nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Dead = table.Column<bool>(type: "boolean", nullable: false),
                    IsNewCompany = table.Column<bool>(type: "boolean", nullable: false),
                    CompanySpecialBilling = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "company");
        }
    }
}
