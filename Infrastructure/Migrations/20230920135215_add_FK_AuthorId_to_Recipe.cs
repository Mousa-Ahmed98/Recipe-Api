using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_FK_AuthorId_to_Recipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteRecipes_AspNetUsers_UserId",
                table: "FavouriteRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteRecipes_Recipes_RecipeId",
                table: "FavouriteRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_AspNetUsers_UserId",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_Recipes_RecipeId",
                table: "Plans");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Recipes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RecipeId1",
                table: "Plans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_AuthorId",
                table: "Recipes",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_RecipeId1",
                table: "Plans",
                column: "RecipeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteRecipes_AspNetUsers_UserId",
                table: "FavouriteRecipes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteRecipes_Recipes_RecipeId",
                table: "FavouriteRecipes",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_AspNetUsers_UserId",
                table: "Plans",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_Recipes_RecipeId",
                table: "Plans",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_Recipes_RecipeId1",
                table: "Plans",
                column: "RecipeId1",
                principalTable: "Recipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_AspNetUsers_AuthorId",
                table: "Recipes",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteRecipes_AspNetUsers_UserId",
                table: "FavouriteRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteRecipes_Recipes_RecipeId",
                table: "FavouriteRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_AspNetUsers_UserId",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_Recipes_RecipeId",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_Recipes_RecipeId1",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_AspNetUsers_AuthorId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_AuthorId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Plans_RecipeId1",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "RecipeId1",
                table: "Plans");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteRecipes_AspNetUsers_UserId",
                table: "FavouriteRecipes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteRecipes_Recipes_RecipeId",
                table: "FavouriteRecipes",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_AspNetUsers_UserId",
                table: "Plans",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_Recipes_RecipeId",
                table: "Plans",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
