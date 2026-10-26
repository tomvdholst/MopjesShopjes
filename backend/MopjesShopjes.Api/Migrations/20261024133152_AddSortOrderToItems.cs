using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MopjesShopjes.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSortOrderToItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "SortOrder",
                table: "Items",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Items");
        }
    }
}
