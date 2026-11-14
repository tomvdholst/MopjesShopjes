using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MopjesShopjes.Api.Migrations
{
    /// <inheritdoc />
    public partial class LinkItemsToCatalogItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Items",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "CatalogItemId",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "Items",
                type: "numeric(8,3)",
                precision: 8,
                scale: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuantityText",
                table: "Items",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_CatalogItemId",
                table: "Items",
                column: "CatalogItemId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Items_NameOrCatalogItem",
                table: "Items",
                sql: "(\"Name\" IS NULL) <> (\"CatalogItemId\" IS NULL)");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_CatalogItems_CatalogItemId",
                table: "Items",
                column: "CatalogItemId",
                principalTable: "CatalogItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_CatalogItems_CatalogItemId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CatalogItemId",
                table: "Items");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Items_NameOrCatalogItem",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CatalogItemId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "QuantityText",
                table: "Items");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Items",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
