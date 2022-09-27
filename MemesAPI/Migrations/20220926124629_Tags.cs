using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemesAPI.Migrations
{
    public partial class Tags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemeLike_Memes_MemeId",
                table: "MemeLike");

            migrationBuilder.DropColumn(
                name: "profilePic",
                table: "Memes");

            migrationBuilder.DropColumn(
                name: "Meme",
                table: "MemeLike");

            migrationBuilder.AlterColumn<int>(
                name: "MemeId",
                table: "MemeLike",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemeUserId",
                table: "MemeLike",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TagMeme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagMeme", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MemeTagMeme",
                columns: table => new
                {
                    MemesId = table.Column<int>(type: "int", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemeTagMeme", x => new { x.MemesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_MemeTagMeme_Memes_MemesId",
                        column: x => x.MemesId,
                        principalTable: "Memes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemeTagMeme_TagMeme_TagsId",
                        column: x => x.TagsId,
                        principalTable: "TagMeme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MemeLike_MemeUserId",
                table: "MemeLike",
                column: "MemeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MemeTagMeme_TagsId",
                table: "MemeTagMeme",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemeLike_AspNetUsers_MemeUserId",
                table: "MemeLike",
                column: "MemeUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MemeLike_Memes_MemeId",
                table: "MemeLike",
                column: "MemeId",
                principalTable: "Memes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemeLike_AspNetUsers_MemeUserId",
                table: "MemeLike");

            migrationBuilder.DropForeignKey(
                name: "FK_MemeLike_Memes_MemeId",
                table: "MemeLike");

            migrationBuilder.DropTable(
                name: "MemeTagMeme");

            migrationBuilder.DropTable(
                name: "TagMeme");

            migrationBuilder.DropIndex(
                name: "IX_MemeLike_MemeUserId",
                table: "MemeLike");

            migrationBuilder.DropColumn(
                name: "MemeUserId",
                table: "MemeLike");

            migrationBuilder.AddColumn<string>(
                name: "profilePic",
                table: "Memes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "MemeId",
                table: "MemeLike",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Meme",
                table: "MemeLike",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_MemeLike_Memes_MemeId",
                table: "MemeLike",
                column: "MemeId",
                principalTable: "Memes",
                principalColumn: "Id");
        }
    }
}
