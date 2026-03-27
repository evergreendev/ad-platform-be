using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_unique_constraint_to_companycontactphone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_company_contact_phone_phone",
                table: "company_contact_phone");

            migrationBuilder.DropIndex(
                name: "ix_company_contact_email_email",
                table: "company_contact_email");

            migrationBuilder.CreateIndex(
                name: "ix_company_contact_phone_phone_company_contact_id",
                table: "company_contact_phone",
                columns: new[] { "phone", "company_contact_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_company_contact_email_email_company_contact_id",
                table: "company_contact_email",
                columns: new[] { "email", "company_contact_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_company_contact_phone_phone_company_contact_id",
                table: "company_contact_phone");

            migrationBuilder.DropIndex(
                name: "ix_company_contact_email_email_company_contact_id",
                table: "company_contact_email");

            migrationBuilder.CreateIndex(
                name: "ix_company_contact_phone_phone",
                table: "company_contact_phone",
                column: "phone");

            migrationBuilder.CreateIndex(
                name: "ix_company_contact_email_email",
                table: "company_contact_email",
                column: "email");
        }
    }
}
