using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_external_map_constraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_external_id_map_source_system_source_table_source_id",
                table: "external_id_map");

            migrationBuilder.CreateIndex(
                name: "ix_external_id_map_source_system_source_table_source_id_entity",
                table: "external_id_map",
                columns: new[] { "source_system", "source_table", "source_id", "entity_type" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_external_id_map_source_system_source_table_source_id_entity",
                table: "external_id_map");

            migrationBuilder.CreateIndex(
                name: "ix_external_id_map_source_system_source_table_source_id",
                table: "external_id_map",
                columns: new[] { "source_system", "source_table", "source_id" },
                unique: true);
        }
    }
}
