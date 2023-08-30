using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "recipes");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "recipes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_recipes_CategoryId",
                table: "recipes",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_recipes_Category_CategoryId",
                table: "recipes",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recipes_Category_CategoryId",
                table: "recipes");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_recipes_CategoryId",
                table: "recipes");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "recipes");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "recipes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
