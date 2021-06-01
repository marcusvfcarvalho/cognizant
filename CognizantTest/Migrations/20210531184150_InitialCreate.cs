using Microsoft.EntityFrameworkCore.Migrations;

namespace CognizantTest.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChallengeTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    TestInputParameter = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    TestOutputParameter = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeTaskSolutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", maxLength: 20000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeTaskSolutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengeTaskSolutions_ChallengeTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "ChallengeTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeTaskSolutions_TaskId",
                table: "ChallengeTaskSolutions",
                column: "TaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallengeTaskSolutions");

            migrationBuilder.DropTable(
                name: "ChallengeTasks");
        }
    }
}