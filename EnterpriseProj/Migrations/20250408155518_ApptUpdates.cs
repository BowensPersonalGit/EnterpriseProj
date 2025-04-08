using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnterpriseProj.Migrations
{
    /// <inheritdoc />
    public partial class ApptUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PractitionerId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PractitionerId",
                table: "Appointments",
                column: "PractitionerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_PractitionerId",
                table: "Appointments",
                column: "PractitionerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Users_PractitionerId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_PractitionerId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PractitionerId",
                table: "Appointments");
        }
    }
}
