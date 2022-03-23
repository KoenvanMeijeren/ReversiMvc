using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReversiMvc.Migrations;

public partial class AddDefaultUser : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "AspNetRoles",
            columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            values: new object[] { "01b92546-0595-43fb-b74b-63041cec49c3", "0550b4cb-2025-4b6e-98bd-e3cf6ad4e3da", "Mediator", "MEDIATOR" });

        migrationBuilder.InsertData(
            table: "AspNetRoles",
            columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            values: new object[] { "37186721-a6e7-418f-8a56-1c3b6596264f", "c62341c3-6fbe-4e1f-bb56-fd8de73d248d", "Admin", "ADMIN" });

        migrationBuilder.InsertData(
            table: "AspNetUsers",
            columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
            values: new object[] { "a18be9c0-aa65-4af8-bd17-00bd9344e575", 0, "52005321-457a-4d4d-988a-1e774c23ee0e", "admin@nimda.com", true, false, null, "ADMIN@NIMDA.COM", "ADMIN@NIMDA.COM", "AQAAAAEAACcQAAAAEIaf6sraeJdePw02NtvgvqENYmRJeDyvmfoAv1QAKN1jYnla2WoZhBr6Aj/EoOyGMQ==", null, false, "", false, "admin@nimda.com" });

        migrationBuilder.InsertData(
            table: "AspNetUserRoles",
            columns: new[] { "RoleId", "UserId" },
            values: new object[] { "37186721-a6e7-418f-8a56-1c3b6596264f", "a18be9c0-aa65-4af8-bd17-00bd9344e575" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "01b92546-0595-43fb-b74b-63041cec49c3");

        migrationBuilder.DeleteData(
            table: "AspNetUserRoles",
            keyColumns: new[] { "RoleId", "UserId" },
            keyValues: new object[] { "37186721-a6e7-418f-8a56-1c3b6596264f", "a18be9c0-aa65-4af8-bd17-00bd9344e575" });

        migrationBuilder.DeleteData(
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "37186721-a6e7-418f-8a56-1c3b6596264f");

        migrationBuilder.DeleteData(
            table: "AspNetUsers",
            keyColumn: "Id",
            keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575");
    }
}
