using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace URLshorten.Migrations
{
    /// <inheritdoc />
    public partial class UrlShortenDbUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "QRImage",
                table: "UrlShortenModel",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QRImage",
                table: "UrlShortenModel");
        }
    }
}
