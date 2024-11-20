using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZBank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IncludedTeamName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeamName",
                table: "Notifications",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamName",
                table: "Notifications");
        }
    }
}
