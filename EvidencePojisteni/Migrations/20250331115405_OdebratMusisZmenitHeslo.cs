using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvidencePojisteni.Migrations
{
    /// <inheritdoc />
    public partial class OdebratMusisZmenitHeslo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MusisZmenitHeslo",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MusisZmenitHeslo",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
