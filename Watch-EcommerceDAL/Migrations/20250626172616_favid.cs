using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Watch_EcommerceDAL.Migrations
{
    /// <inheritdoc />
    public partial class favid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FavId",
                table: "Favourites",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavId",
                table: "Favourites");
        }
    }
}
