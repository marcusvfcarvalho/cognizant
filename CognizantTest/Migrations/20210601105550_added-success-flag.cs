using Microsoft.EntityFrameworkCore.Migrations;

namespace CognizantTest.Api.Migrations
{
    public partial class addedsuccessflag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Success",
                table: "ChallengeTaskSolutions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Success",
                table: "ChallengeTaskSolutions");
        }
    }
}