using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class SeedAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "LastLoginDate", "Mail", "Name", "Password" },
                values: new object[] { 1, new DateTime(2024, 9, 1, 0, 29, 59, 377, DateTimeKind.Utc).AddTicks(9460), new DateTime(2024, 9, 1, 0, 29, 59, 377, DateTimeKind.Utc).AddTicks(9461), "kiosko@example.com", "Pedro Paramo", "$2a$11$rxziVW5vnCWEh74E0RfTR.ybxTCWX4LHiUUPN3amac7ZuZproWiOK" });

            migrationBuilder.InsertData(
                table: "Feeds",
                columns: new[] { "Id", "Description", "IsPrivate", "Name", "UserId" },
                values: new object[] { 1, "All about sports", false, "Sports", 1 });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "Description", "FeedId", "Name" },
                values: new object[,]
                {
                    { 1, "Swimming News", 1, "Swimming" },
                    { 2, "Cycling News", 1, "Cycling" },
                    { 3, "Tennis News", 1, "Tennis" },
                    { 4, "Boxing News", 1, "Boxing" },
                    { 5, "Shooting News", 1, "Shooting" },
                    { 6, "Equestrian News", 1, "Equestrian" },
                    { 7, "Jumping News", 1, "Jumping" },
                    { 8, "Sailing News", 1, "Sailing" },
                    { 9, "Rhythmic News", 1, "Rhythmic" },
                    { 10, "Gymnastics News", 1, "Gymnastics" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Feeds",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
