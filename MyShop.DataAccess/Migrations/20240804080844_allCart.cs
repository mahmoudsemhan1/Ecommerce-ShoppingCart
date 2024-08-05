using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop.Web.Migrations
{
    /// <inheritdoc />
    public partial class allCart : Migration
    {
        /// <inheritdoc />
      
            protected override void Up(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.CreateTable(
                    name: "Carts",
                    columns: table => new
                    {
                        Id = table.Column<int>(nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        ApplicationUserId = table.Column<string>(nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Carts", x => x.Id);
                        table.ForeignKey(
                            name: "FK_Carts_AspNetUsers_ApplicationUserId",
                            column: x => x.ApplicationUserId,
                            principalTable: "AspNetUsers",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict);
                    });

                migrationBuilder.CreateTable(
                    name: "CartItems",
                    columns: table => new
                    {
                        CartItemId = table.Column<int>(nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        ProductId = table.Column<int>(nullable: false),
                        Quantity = table.Column<int>(nullable: false),
                        CartId = table.Column<int>(nullable: false),
                        ApplicationUserId = table.Column<string>(nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_CartItems", x => x.CartItemId);
                        table.ForeignKey(
                            name: "FK_CartItems_Carts_CartId",
                            column: x => x.CartId,
                            principalTable: "Carts",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                        table.ForeignKey(
                            name: "FK_CartItems_Products_ProductId",
                            column: x => x.ProductId,
                            principalTable: "Products",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    });

                migrationBuilder.CreateIndex(
                    name: "IX_CartItems_CartId",
                    table: "CartItems",
                    column: "CartId");

                migrationBuilder.CreateIndex(
                    name: "IX_CartItems_ProductId",
                    table: "CartItems",
                    column: "ProductId");

                migrationBuilder.CreateIndex(
                    name: "IX_Carts_ApplicationUserId",
                    table: "Carts",
                    column: "ApplicationUserId");
            }

            protected override void Down(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.DropTable(
                    name: "CartItems");

                migrationBuilder.DropTable(
                    name: "Carts");
            }
        

    }
}
