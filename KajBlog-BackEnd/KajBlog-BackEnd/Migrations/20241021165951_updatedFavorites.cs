using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KajBlog_BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class updatedFavorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Favorites_BlogId",
                table: "Favorites",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Blogs_BlogId",
                table: "Favorites",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "BlogId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Blogs_BlogId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_BlogId",
                table: "Favorites");
        }
    }
}
