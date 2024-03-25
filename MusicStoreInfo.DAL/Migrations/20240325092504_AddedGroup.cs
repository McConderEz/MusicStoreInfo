using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicStoreInfo.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Groups");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Groups");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Groups",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
