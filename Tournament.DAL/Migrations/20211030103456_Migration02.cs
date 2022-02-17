using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.DAL.Migrations
{
    public partial class Migration02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Places",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("ae6d83ab-1688-4a6f-8bbb-748aeb5fc96b"), "Another one match place.", "Boulevard" });

            migrationBuilder.UpdateData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: new Guid("2b4450d3-c32e-45af-80ac-8ec0cb5a6537"),
                column: "RegistrationCountry",
                value: 203);

            migrationBuilder.UpdateData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: new Guid("74738348-08a6-4c25-9b93-219d88c2de2d"),
                column: "RegistrationCountry",
                value: 60);

            migrationBuilder.InsertData(
                table: "Matches",
                columns: new[] { "Id", "DateTime", "PlaceId", "Result", "Team1Id", "Team2Id" },
                values: new object[] { new Guid("819a4cf7-e06d-42b5-a1e9-47cceb48424d"), new DateTime(2021, 10, 16, 17, 56, 20, 501, DateTimeKind.Local).AddTicks(2002), new Guid("ae6d83ab-1688-4a6f-8bbb-748aeb5fc96b"), 3, new Guid("74738348-08a6-4c25-9b93-219d88c2de2d"), new Guid("2b4450d3-c32e-45af-80ac-8ec0cb5a6537") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("819a4cf7-e06d-42b5-a1e9-47cceb48424d"));

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: new Guid("ae6d83ab-1688-4a6f-8bbb-748aeb5fc96b"));

            migrationBuilder.UpdateData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: new Guid("2b4450d3-c32e-45af-80ac-8ec0cb5a6537"),
                column: "RegistrationCountry",
                value: 61);

            migrationBuilder.UpdateData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: new Guid("74738348-08a6-4c25-9b93-219d88c2de2d"),
                column: "RegistrationCountry",
                value: 61);
        }
    }
}
