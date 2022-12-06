using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Strider.Posterr.RelationalData.Migrations
{
    /// <inheritdoc />
    public partial class seedusers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedOn", "UserName" },
                values: new object[,]
                {
                    { new Guid("215b5bd8-8d90-434b-b13f-1391da1bb245"), new DateTime(2022, 12, 1, 10, 33, 9, 484, DateTimeKind.Local).AddTicks(7109), "User-Two" },
                    { new Guid("eed1e1eb-51a2-4f33-9b97-401acf10d5a8"), new DateTime(2022, 12, 2, 10, 33, 9, 484, DateTimeKind.Local).AddTicks(7096), "User-One" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("215b5bd8-8d90-434b-b13f-1391da1bb245"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("eed1e1eb-51a2-4f33-9b97-401acf10d5a8"));
        }
    }
}
