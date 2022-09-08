using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemesAPI.Migrations
{
    public partial class authgoogle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemeMemeUser");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Memes",
                type: "varchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Memes",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MemeUserId",
                table: "Memes",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "profilePic",
                table: "Memes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MemeLike",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "MemeId",
                table: "MemeLike",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "profilePic",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemeLike",
                table: "MemeLike",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Memes_MemeUserId",
                table: "Memes",
                column: "MemeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MemeLike_MemeId",
                table: "MemeLike",
                column: "MemeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemeLike_Memes_MemeId",
                table: "MemeLike",
                column: "MemeId",
                principalTable: "Memes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Memes_AspNetUsers_MemeUserId",
                table: "Memes",
                column: "MemeUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemeLike_Memes_MemeId",
                table: "MemeLike");

            migrationBuilder.DropForeignKey(
                name: "FK_Memes_AspNetUsers_MemeUserId",
                table: "Memes");

            migrationBuilder.DropIndex(
                name: "IX_Memes_MemeUserId",
                table: "Memes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemeLike",
                table: "MemeLike");

            migrationBuilder.DropIndex(
                name: "IX_MemeLike_MemeId",
                table: "MemeLike");

            migrationBuilder.DropColumn(
                name: "MemeUserId",
                table: "Memes");

            migrationBuilder.DropColumn(
                name: "profilePic",
                table: "Memes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MemeLike");

            migrationBuilder.DropColumn(
                name: "MemeId",
                table: "MemeLike");

            migrationBuilder.DropColumn(
                name: "profilePic",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Memes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(60)",
                oldMaxLength: 60)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Memes",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MemeMemeUser",
                columns: table => new
                {
                    LikesId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MemeUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemeMemeUser", x => new { x.LikesId, x.MemeUser });
                    table.ForeignKey(
                        name: "FK_MemeMemeUser_AspNetUsers_LikesId",
                        column: x => x.LikesId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemeMemeUser_Memes_MemeUser",
                        column: x => x.MemeUser,
                        principalTable: "Memes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MemeMemeUser_MemeUser",
                table: "MemeMemeUser",
                column: "MemeUser");
        }
    }
}
