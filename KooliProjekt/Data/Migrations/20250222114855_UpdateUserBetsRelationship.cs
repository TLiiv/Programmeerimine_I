using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KooliProjekt.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserBetsRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersBets_Users_UserId",
                table: "UsersBets");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersBets_Users_UserId",
                table: "UsersBets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersBets_Users_UserId",
                table: "UsersBets");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersBets_Users_UserId",
                table: "UsersBets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
