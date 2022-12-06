using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Strider.Posterr.RelationalData.Migrations
{
    /// <inheritdoc />
    public partial class navigationpropertyadjust : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_OriginalPostId",
                table: "Posts");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("215b5bd8-8d90-434b-b13f-1391da1bb245"),
                column: "CreatedOn",
                value: new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("eed1e1eb-51a2-4f33-9b97-401acf10d5a8"),
                column: "CreatedOn",
                value: new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Posts_OriginalPostId",
                table: "Posts",
                column: "OriginalPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_OriginalPostId",
                table: "Posts");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("215b5bd8-8d90-434b-b13f-1391da1bb245"),
                column: "CreatedOn",
                value: new DateTime(2022, 12, 1, 10, 33, 9, 484, DateTimeKind.Local).AddTicks(7109));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("eed1e1eb-51a2-4f33-9b97-401acf10d5a8"),
                column: "CreatedOn",
                value: new DateTime(2022, 12, 2, 10, 33, 9, 484, DateTimeKind.Local).AddTicks(7096));

            migrationBuilder.CreateIndex(
                name: "IX_Posts_OriginalPostId",
                table: "Posts",
                column: "OriginalPostId",
                unique: true,
                filter: "[OriginalPostId] IS NOT NULL");
        }
    }
}
