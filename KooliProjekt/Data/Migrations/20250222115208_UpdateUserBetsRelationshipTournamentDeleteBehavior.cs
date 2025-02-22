using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KooliProjekt.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserBetsRelationshipTournamentDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersBets_Tournaments_TournamentId",
                table: "UsersBets");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersBets_Tournaments_TournamentId",
                table: "UsersBets",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "TournamentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersBets_Tournaments_TournamentId",
                table: "UsersBets");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersBets_Tournaments_TournamentId",
                table: "UsersBets",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "TournamentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
