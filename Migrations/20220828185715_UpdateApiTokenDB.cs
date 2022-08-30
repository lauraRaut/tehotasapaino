using Microsoft.EntityFrameworkCore.Migrations;

namespace Tehotasapaino.Migrations
{
    public partial class UpdateApiTokenDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastRefreshTimeStamp",
                table: "UserExternalAPITokens",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "ExpirationTime",
                table: "UserExternalAPITokens",
                newName: "Expires_in");

            migrationBuilder.RenameColumn(
                name: "APIToken",
                table: "UserExternalAPITokens",
                newName: "UserNameProvider");

            migrationBuilder.AddColumn<string>(
                name: "Access_token",
                table: "UserExternalAPITokens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Refresh_token",
                table: "UserExternalAPITokens",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Access_token",
                table: "UserExternalAPITokens");

            migrationBuilder.DropColumn(
                name: "Refresh_token",
                table: "UserExternalAPITokens");

            migrationBuilder.RenameColumn(
                name: "UserNameProvider",
                table: "UserExternalAPITokens",
                newName: "APIToken");

            migrationBuilder.RenameColumn(
                name: "Expires_in",
                table: "UserExternalAPITokens",
                newName: "ExpirationTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "UserExternalAPITokens",
                newName: "LastRefreshTimeStamp");
        }
    }
}
