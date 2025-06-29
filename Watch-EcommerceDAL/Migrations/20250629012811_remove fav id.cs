using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Watch_EcommerceDAL.Migrations
{
    /// <inheritdoc />
    public partial class removefavid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavId",
                table: "Favourites");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FavId",
                table: "Favourites",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
