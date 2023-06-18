using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageMicroService.Infrastructure.Migrations
{
    public partial class GallerySetNullMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Galleries_GalleryId",
                table: "Images");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Galleries_GalleryId",
                table: "Images",
                column: "GalleryId",
                principalTable: "Galleries",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Galleries_GalleryId",
                table: "Images");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Galleries_GalleryId",
                table: "Images",
                column: "GalleryId",
                principalTable: "Galleries",
                principalColumn: "Id");
        }
    }
}
