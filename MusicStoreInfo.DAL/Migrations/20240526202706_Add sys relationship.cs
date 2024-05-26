using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicStoreInfo.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Addsysrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PrincipalId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PrincipalId",
                table: "Roles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrincipalId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PrincipalId",
                table: "Roles");
        }
    }
}
