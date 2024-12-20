using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inzynierka.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Dodanie unikalnych indeksów
            migrationBuilder.CreateIndex(
                name: "IX_Contractors_Email",
                table: "Contractors",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contractors_PhoneNumber",
                table: "Contractors",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contractors_TaxIdNumber",
                table: "Contractors",
                column: "TaxIdNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Usunięcie unikalnych indeksów w razie cofnięcia migracji
            migrationBuilder.DropIndex(
                name: "IX_Contractors_Email",
                table: "Contractors");

            migrationBuilder.DropIndex(
                name: "IX_Contractors_PhoneNumber",
                table: "Contractors");

            migrationBuilder.DropIndex(
                name: "IX_Contractors_TaxIdNumber",
                table: "Contractors");
        }
    }
}
