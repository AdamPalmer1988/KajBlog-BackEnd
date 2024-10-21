using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KajBlog_BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "BlogId", "BlogBody", "Category", "CreatedBy", "CreatedOn", "GiphyPull", "SubjectLine", "UpdatedBy", "UpdatedOn", "UserId" },
                values: new object[,]
                {
                    { 1, "C# 10 introduces a number of exciting features, such as global usings and file-scoped namespaces.", "Tech", "user1", new DateTime(2024, 10, 21, 21, 24, 19, 963, DateTimeKind.Utc).AddTicks(7928), "https://giphy.com/some-url", "Exploring C# 10 Features", null, null, "user1" },
                    { 2, "If you’re looking for the next best travel spots, check out these incredible destinations for 2024.", "Travel", "user2", new DateTime(2024, 10, 21, 21, 24, 19, 963, DateTimeKind.Utc).AddTicks(7930), "https://giphy.com/some-url-2", "Top 10 Destinations for 2024", null, null, "user2" },
                    { 3, "Mindfulness can help you stay in the present moment and reduce stress. Here's how to get started.", "Health", "user3", new DateTime(2024, 10, 21, 21, 24, 19, 963, DateTimeKind.Utc).AddTicks(7931), "https://giphy.com/some-url-3", "Mindfulness Techniques for Beginners", null, null, "user3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "BlogId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "BlogId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "BlogId",
                keyValue: 3);
        }
    }
}
