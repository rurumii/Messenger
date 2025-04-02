using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class DTOs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UsernameTag",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UsernameTag",
                table: "Users",
                newName: "UserTag");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserTag",
                table: "Users",
                column: "UserTag",
                unique: true,
                filter: "[UserTag] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserTag",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserTag",
                table: "Users",
                newName: "UsernameTag");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UsernameTag",
                table: "Users",
                column: "UsernameTag",
                unique: true,
                filter: "[UsernameTag] IS NOT NULL");
        }
    }
}
