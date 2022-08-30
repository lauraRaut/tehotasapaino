using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tehotasapaino.Migrations
{
    public partial class AddedConsumptionDataAndUserTokenModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserConsumptionData",
                columns: table => new
                {
                    UserElectricityConsumptionDataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserInformationId = table.Column<int>(type: "int", nullable: false),
                    WeekNum = table.Column<int>(type: "int", nullable: false),
                    WeekDay = table.Column<int>(type: "int", nullable: false),
                    Hour = table.Column<int>(type: "int", nullable: false),
                    AverageConsumptionkWh = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConsumptionData", x => x.UserElectricityConsumptionDataId);
                    table.ForeignKey(
                        name: "FK_UserConsumptionData_UserData_UserInformationId",
                        column: x => x.UserInformationId,
                        principalTable: "UserData",
                        principalColumn: "UserInformationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserExternalAPITokens",
                columns: table => new
                {
                    UserExternalAPITokenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserInformationId = table.Column<int>(type: "int", nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    APIToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastRefreshTimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExternalAPITokens", x => x.UserExternalAPITokenId);
                    table.ForeignKey(
                        name: "FK_UserExternalAPITokens_UserData_UserInformationId",
                        column: x => x.UserInformationId,
                        principalTable: "UserData",
                        principalColumn: "UserInformationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserConsumptionData_UserInformationId",
                table: "UserConsumptionData",
                column: "UserInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExternalAPITokens_UserInformationId",
                table: "UserExternalAPITokens",
                column: "UserInformationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserConsumptionData");

            migrationBuilder.DropTable(
                name: "UserExternalAPITokens");
        }
    }
}
