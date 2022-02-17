using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.DAL.Migrations
{
    public partial class Migration03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: new Guid("2b4450d3-c32e-45af-80ac-8ec0cb5a6537"),
                column: "LogoURL",
                value: "other.url/2");

            migrationBuilder.UpdateData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: new Guid("74738348-08a6-4c25-9b93-219d88c2de2d"),
                column: "LogoURL",
                value: "some.url/1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: new Guid("2b4450d3-c32e-45af-80ac-8ec0cb5a6537"),
                column: "LogoURL",
                value: "");

            migrationBuilder.UpdateData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: new Guid("74738348-08a6-4c25-9b93-219d88c2de2d"),
                column: "LogoURL",
                value: "");
        }
    }
}
