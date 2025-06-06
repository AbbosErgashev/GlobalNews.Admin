using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace News.Admin.Migrations
{
    /// <inheritdoc />
    public partial class addedUpdatedAtToModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "NewsItems",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "NewsItems");
        }
    }
}
