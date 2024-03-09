using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasksService.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddJiraId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "jira_id",
                table: "tasks",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "jira_id",
                table: "tasks");
        }
    }
}
