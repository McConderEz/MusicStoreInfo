using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicStoreInfo.DAL.Migrations
{
    /// <inheritdoc />
    public partial class NewFileSaver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Albums");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Albums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Albums");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Albums",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
