using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class migratenew1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_url",
                table: "Inventories");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Inventories",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Inventories");

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "Inventories",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
