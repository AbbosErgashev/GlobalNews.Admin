using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace News.Admin.Migrations
{
    /// <inheritdoc />
    public partial class RemovedItemFromCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsItems_Categories_CategoryId",
                table: "NewsItems");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsItems_Categories_CategoryId",
                table: "NewsItems",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsItems_Categories_CategoryId",
                table: "NewsItems");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsItems_Categories_CategoryId",
                table: "NewsItems",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
