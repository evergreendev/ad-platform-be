using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class remove_company_properties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LegacyBillingContact",
                table: "company");

            migrationBuilder.DropColumn(
                name: "LegacyPrimaryContact",
                table: "company");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LegacyBillingContact",
                table: "company",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegacyPrimaryContact",
                table: "company",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
