using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Watch_EcommerceDAL.Migrations
{
    /// <inheritdoc />
    public partial class addOrderAddressGovernorateDeliverymethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryMethodId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrderAddress_City",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrderAddress_FirstName",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OrderAddress_GovernorateId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrderAddress_LastName",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrderAddress_Street",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Deliverymethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliverymethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Governorates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GovernorateDeliveryMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GovernorateId = table.Column<int>(type: "int", nullable: false),
                    DeliveryMethodId = table.Column<int>(type: "int", nullable: false),
                    DeliveryCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GovernorateDeliveryMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GovernorateDeliveryMethods_Deliverymethods_DeliveryMethodId",
                        column: x => x.DeliveryMethodId,
                        principalTable: "Deliverymethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GovernorateDeliveryMethods_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_DeliveryMethodId",
                table: "Order",
                column: "DeliveryMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderAddress_GovernorateId",
                table: "Order",
                column: "OrderAddress_GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_GovernorateDeliveryMethods_DeliveryMethodId",
                table: "GovernorateDeliveryMethods",
                column: "DeliveryMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_GovernorateDeliveryMethods_GovernorateId",
                table: "GovernorateDeliveryMethods",
                column: "GovernorateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Deliverymethods_DeliveryMethodId",
                table: "Order",
                column: "DeliveryMethodId",
                principalTable: "Deliverymethods",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Governorates_OrderAddress_GovernorateId",
                table: "Order",
                column: "OrderAddress_GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Deliverymethods_DeliveryMethodId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Governorates_OrderAddress_GovernorateId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "GovernorateDeliveryMethods");

            migrationBuilder.DropTable(
                name: "Deliverymethods");

            migrationBuilder.DropTable(
                name: "Governorates");

            migrationBuilder.DropIndex(
                name: "IX_Order_DeliveryMethodId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_OrderAddress_GovernorateId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DeliveryMethodId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderAddress_City",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderAddress_FirstName",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderAddress_GovernorateId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderAddress_LastName",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderAddress_Street",
                table: "Order");
        }
    }
}
