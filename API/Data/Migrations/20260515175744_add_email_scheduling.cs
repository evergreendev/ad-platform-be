using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_email_scheduling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "attempt_count",
                table: "email_message",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "cancelled_at",
                table: "email_message",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "failed_at",
                table: "email_message",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_error",
                table: "email_message",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "scheduled_for",
                table: "email_message",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "scheduler_job_id",
                table: "email_message",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_email_message_scheduled_for",
                table: "email_message",
                column: "scheduled_for");

            migrationBuilder.CreateIndex(
                name: "ix_email_message_scheduler_job_id",
                table: "email_message",
                column: "scheduler_job_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_email_message_scheduled_for",
                table: "email_message");

            migrationBuilder.DropIndex(
                name: "ix_email_message_scheduler_job_id",
                table: "email_message");

            migrationBuilder.DropColumn(
                name: "attempt_count",
                table: "email_message");

            migrationBuilder.DropColumn(
                name: "cancelled_at",
                table: "email_message");

            migrationBuilder.DropColumn(
                name: "failed_at",
                table: "email_message");

            migrationBuilder.DropColumn(
                name: "last_error",
                table: "email_message");

            migrationBuilder.DropColumn(
                name: "scheduled_for",
                table: "email_message");

            migrationBuilder.DropColumn(
                name: "scheduler_job_id",
                table: "email_message");
        }
    }
}
