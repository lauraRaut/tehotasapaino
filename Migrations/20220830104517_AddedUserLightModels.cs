using Microsoft.EntityFrameworkCore.Migrations;

namespace Tehotasapaino.Migrations
{
    public partial class AddedUserLightModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAlertLightInformation",
                columns: table => new
                {
                    UserAlertLightInformationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserInformationId = table.Column<int>(type: "int", nullable: false),
                    LightGUID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LightAlertHexColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LightBeforeAlertHexColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlertPriceLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAlertLightInformation", x => x.UserAlertLightInformationId);
                    table.ForeignKey(
                        name: "FK_UserAlertLightInformation_UserData_UserInformationId",
                        column: x => x.UserInformationId,
                        principalTable: "UserData",
                        principalColumn: "UserInformationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAlertLightInformation_UserInformationId",
                table: "UserAlertLightInformation",
                column: "UserInformationId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAlertLightInformation");
        }
    }
}
