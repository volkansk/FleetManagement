using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetManagement.Repository.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Plate = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barcode = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    DeliveryPointId = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bags_DeliveryPoints_DeliveryPointId",
                        column: x => x.DeliveryPointId,
                        principalTable: "DeliveryPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FleetTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    DeliveryPointId = table.Column<int>(type: "int", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FleetTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FleetTransactions_DeliveryPoints_DeliveryPointId",
                        column: x => x.DeliveryPointId,
                        principalTable: "DeliveryPoints",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FleetTransactions_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barcode = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    DeliveryPointId = table.Column<int>(type: "int", nullable: false),
                    VolumetricWeight = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    BagId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Packages_Bags_BagId",
                        column: x => x.BagId,
                        principalTable: "Bags",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Packages_DeliveryPoints_DeliveryPointId",
                        column: x => x.DeliveryPointId,
                        principalTable: "DeliveryPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bags_Barcode",
                table: "Bags",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bags_DeliveryPointId",
                table: "Bags",
                column: "DeliveryPointId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryPoints_Value",
                table: "DeliveryPoints",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FleetTransactions_DeliveryPointId",
                table: "FleetTransactions",
                column: "DeliveryPointId");

            migrationBuilder.CreateIndex(
                name: "IX_FleetTransactions_VehicleId",
                table: "FleetTransactions",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_BagId",
                table: "Packages",
                column: "BagId");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_Barcode",
                table: "Packages",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Packages_DeliveryPointId",
                table: "Packages",
                column: "DeliveryPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_Plate",
                table: "Vehicles",
                column: "Plate",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FleetTransactions");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Bags");

            migrationBuilder.DropTable(
                name: "DeliveryPoints");
        }
    }
}
