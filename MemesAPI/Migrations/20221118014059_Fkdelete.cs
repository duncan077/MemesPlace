using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemesAPI.Migrations
{
    public partial class Fkdelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memes_AspNetUsers_UserId",
                table: "Memes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "043377e4-9f2c-42d3-9d02-88ea5adcfae7",
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEKIG4UN7r8rEZnvw3RCt7FA7D6t4p7QFgMWKsK0yLkMSxkVU326TXi1jcJ2EFWYA2g==");

            migrationBuilder.AddForeignKey(
                name: "FK_Memes_AspNetUsers_UserId",
                table: "Memes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memes_AspNetUsers_UserId",
                table: "Memes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "043377e4-9f2c-42d3-9d02-88ea5adcfae7",
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEOh6WZnRMGgwvJsjAZh9cR6NF5m2/Uc6OJuFAKt1rvJhGCEQu1R4h7cmWQkzuRj/oQ==");

            migrationBuilder.AddForeignKey(
                name: "FK_Memes_AspNetUsers_UserId",
                table: "Memes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
