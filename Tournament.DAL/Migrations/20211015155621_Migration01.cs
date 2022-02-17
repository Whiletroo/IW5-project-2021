using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.DAL.Migrations
{
    public partial class Migration01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationCountry = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Team1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Team2Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Result = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_Team1Id",
                        column: x => x.Team1Id,
                        principalTable: "Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Matches_Teams_Team2Id",
                        column: x => x.Team2Id,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Places",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("11f3a641-0404-40cf-83b5-80e293062eb1"), "This is an event place.", "Avenue" });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "Description", "LogoURL", "RegistrationCountry", "TeamName" },
                values: new object[] { new Guid("2b4450d3-c32e-45af-80ac-8ec0cb5a6537"), "This is a team 2", "", 61, "Team Woman" });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "Description", "LogoURL", "RegistrationCountry", "TeamName" },
                values: new object[] { new Guid("74738348-08a6-4c25-9b93-219d88c2de2d"), "This is a team 1", "", 61, "Team Man" });

            migrationBuilder.InsertData(
                table: "Matches",
                columns: new[] { "Id", "DateTime", "PlaceId", "Result", "Team1Id", "Team2Id" },
                values: new object[] { new Guid("df2e39d9-f691-4f9b-8533-61c2474c23f7"), new DateTime(2021, 10, 15, 17, 56, 20, 501, DateTimeKind.Local).AddTicks(2002), new Guid("11f3a641-0404-40cf-83b5-80e293062eb1"), 3, new Guid("74738348-08a6-4c25-9b93-219d88c2de2d"), new Guid("2b4450d3-c32e-45af-80ac-8ec0cb5a6537") });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Description", "FirstName", "LastName", "PhotoURL", "TeamId" },
                values: new object[] { new Guid("16fc8580-707a-42a2-8ba9-191c67d591de"), "This is a man named John Doe.", "John", "Doe", "", new Guid("74738348-08a6-4c25-9b93-219d88c2de2d") });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Description", "FirstName", "LastName", "PhotoURL", "TeamId" },
                values: new object[] { new Guid("393b47d4-b93f-4b2f-87ad-b905c958067a"), "This is a woman named Jane Sue.", "Jane", "Sue", "", new Guid("2b4450d3-c32e-45af-80ac-8ec0cb5a6537") });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_PlaceId",
                table: "Matches",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Team1Id",
                table: "Matches",
                column: "Team1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Team2Id",
                table: "Matches",
                column: "Team2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_TeamId",
                table: "Persons",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
