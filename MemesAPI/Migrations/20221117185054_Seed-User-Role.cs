using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MemesAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7f2443ad-1ee4-4133-858d-131e4137403e", null, "Admin", "ADMIN" },
                    { "cf10ba0a-51dc-4c53-b6de-0f9f283fff74", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "Karma", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "profilePic", "signature" },
                values: new object[] { "043377e4-9f2c-42d3-9d02-88ea5adcfae7", 0, null, "MemeUser", "duncancacerescartasso@gmail.com", true, 1000, false, null, "DUNCANCACERESCARTASSO@GMAIL.COM", "DUNCAN088", "AQAAAAIAAYagAAAAEGC/1t32AbjbecWWkAHmPNgTwNjeTxMqLhcTN+YjD3fWnUIpiD0a8m/MFwjW3cGMpQ==", null, false, null, false, "duncan088", "", "" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "7f2443ad-1ee4-4133-858d-131e4137403e", "043377e4-9f2c-42d3-9d02-88ea5adcfae7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf10ba0a-51dc-4c53-b6de-0f9f283fff74");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7f2443ad-1ee4-4133-858d-131e4137403e", "043377e4-9f2c-42d3-9d02-88ea5adcfae7" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7f2443ad-1ee4-4133-858d-131e4137403e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "043377e4-9f2c-42d3-9d02-88ea5adcfae7");
        }
    }
}
