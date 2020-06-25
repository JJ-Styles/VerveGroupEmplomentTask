using Microsoft.EntityFrameworkCore.Migrations;

namespace VerveGroupTask.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Login = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Location = table.Column<string>(nullable: false),
                    Avatar_Url = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Login);
                });

            migrationBuilder.CreateTable(
                name: "Repos",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Full_Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Stargazers_Count = table.Column<int>(nullable: false),
                    Svn_Url = table.Column<string>(nullable: false),
                    UserLogin = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Repos_Users_UserLogin",
                        column: x => x.UserLogin,
                        principalTable: "Users",
                        principalColumn: "Login",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stargazers",
                columns: table => new
                {
                    Login = table.Column<string>(nullable: false),
                    RepoID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stargazers", x => x.Login);
                    table.ForeignKey(
                        name: "FK_Stargazers_Repos_RepoID",
                        column: x => x.RepoID,
                        principalTable: "Repos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Repos_UserLogin",
                table: "Repos",
                column: "UserLogin");

            migrationBuilder.CreateIndex(
                name: "IX_Stargazers_RepoID",
                table: "Stargazers",
                column: "RepoID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stargazers");

            migrationBuilder.DropTable(
                name: "Repos");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
