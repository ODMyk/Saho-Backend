using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.SQL.Migrations
{
    /// <inheritdoc />
    public partial class Migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                schema: "public",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "HasProfilePicture",
                schema: "public",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SecurityCode",
                schema: "public",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasCover",
                schema: "public",
                table: "Songs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                schema: "public",
                table: "Songs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasCover",
                schema: "public",
                table: "Playlists",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasCover",
                schema: "public",
                table: "Albums",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasProfilePicture",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecurityCode",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HasCover",
                schema: "public",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                schema: "public",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "HasCover",
                schema: "public",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "HasCover",
                schema: "public",
                table: "Albums");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                schema: "public",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
