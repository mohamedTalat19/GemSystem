using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixConstraintEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "EmailConstraint1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "EmailConstraint",
                table: "Members");

            migrationBuilder.AddCheckConstraint(
                name: "EmailConstraint1",
                table: "Trainers",
                sql: "Email Like '_%@_%._%'");

            migrationBuilder.AddCheckConstraint(
                name: "EmailConstraint",
                table: "Members",
                sql: "Email Like '_%@_%._%'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "EmailConstraint1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "EmailConstraint",
                table: "Members");

            migrationBuilder.AddCheckConstraint(
                name: "EmailConstraint1",
                table: "Trainers",
                sql: "Email Like '_%@_.%'");

            migrationBuilder.AddCheckConstraint(
                name: "EmailConstraint",
                table: "Members",
                sql: "Email Like '_%@_.%'");
        }
    }
}
