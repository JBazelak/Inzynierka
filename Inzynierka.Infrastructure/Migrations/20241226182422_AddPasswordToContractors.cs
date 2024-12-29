using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inzynierka.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordToContractors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Contractors",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Contractors");
        }
    }
}
