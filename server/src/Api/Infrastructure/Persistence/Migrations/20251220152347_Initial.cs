using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Scope = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    HashedPassword = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PermissionId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("0e9b858f-164d-4c75-a559-9c13d3794547"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows the user to view their roles.", "View my roles" },
                    { new Guid("37fad974-167b-4d6f-9cc6-c57b488b72a7"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows the user to update their details.", "Update my details" },
                    { new Guid("3fc973af-c16f-4a92-a461-3cce8f5cecf9"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows the user to delete their details.", "Delete my details" },
                    { new Guid("6b8a28d1-d8eb-46d4-9946-b6007dbb7c23"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows the user to view their details.", "View my details" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Scope" },
                values: new object[,]
                {
                    { new Guid("76477f30-8761-49eb-94ce-5685af2112d6"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows a user to perform tasks which require elevated permissions.", "Super Administrator", 0 },
                    { new Guid("da91b68a-e3bf-4f88-8a72-382a9b868759"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Prevents a user from performing write actions in the application.", "Read-Only User", 0 },
                    { new Guid("ec9607b4-eeb3-4fa2-bb21-0a728ced03f1"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows a user to perform most actions of the application.", "Standard User", 0 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Email", "FirstName", "HashedPassword", "IsDeleted", "LastName" },
                values: new object[,]
                {
                    { new Guid("228d0a28-fe99-4c14-9e4c-d43b5feb02ed"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "alex.morgan@example.com", "Alex", "211D96B96501A67DF5A3133554554F8BA526F66DD2185CE9E3EDA6C6D51A4DC5-49845E168BF607D5952C745D18899D09", false, "Morgan" },
                    { new Guid("60345af2-506f-45d9-bc6d-1e5d16a0e105"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "h.buxton@wearewattle.com", "Harry", "C4F4270B5689DA8BE30D0CD9BFD6BB3FF18E9EA08EDAC567CB2FBCCAFB21B0B5-3B1D5C556AF69604D16EB2C3B5821AB8", false, "Buxton" },
                    { new Guid("929bfd3f-148a-4e2e-85d7-411de090ebfe"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "jordan.alvarez@example.com", "Jordan", "211D96B96501A67DF5A3133554554F8BA526F66DD2185CE9E3EDA6C6D51A4DC5-49845E168BF607D5952C745D18899D09", false, "Alvarez" },
                    { new Guid("9f6bb990-edb9-41fc-8f5e-ec63fa3e4d3f"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "jamie.patel@example.com", "Jamie", "211D96B96501A67DF5A3133554554F8BA526F66DD2185CE9E3EDA6C6D51A4DC5-49845E168BF607D5952C745D18899D09", false, "Patel" },
                    { new Guid("aa5bed02-852a-4a88-a107-159fbab169c2"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "taylor.nguyen@example.com", "Taylor", "211D96B96501A67DF5A3133554554F8BA526F66DD2185CE9E3EDA6C6D51A4DC5-49845E168BF607D5952C745D18899D09", false, "Nguyen" },
                    { new Guid("cdd9d41a-161e-4dd5-a9f4-d521233f03b7"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "chris.reynolds@example.com", "Chris", "211D96B96501A67DF5A3133554554F8BA526F66DD2185CE9E3EDA6C6D51A4DC5-49845E168BF607D5952C745D18899D09", false, "Reynolds" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("0e9b858f-164d-4c75-a559-9c13d3794547"), new Guid("76477f30-8761-49eb-94ce-5685af2112d6") },
                    { new Guid("37fad974-167b-4d6f-9cc6-c57b488b72a7"), new Guid("76477f30-8761-49eb-94ce-5685af2112d6") },
                    { new Guid("3fc973af-c16f-4a92-a461-3cce8f5cecf9"), new Guid("76477f30-8761-49eb-94ce-5685af2112d6") },
                    { new Guid("6b8a28d1-d8eb-46d4-9946-b6007dbb7c23"), new Guid("76477f30-8761-49eb-94ce-5685af2112d6") },
                    { new Guid("0e9b858f-164d-4c75-a559-9c13d3794547"), new Guid("da91b68a-e3bf-4f88-8a72-382a9b868759") },
                    { new Guid("6b8a28d1-d8eb-46d4-9946-b6007dbb7c23"), new Guid("da91b68a-e3bf-4f88-8a72-382a9b868759") },
                    { new Guid("0e9b858f-164d-4c75-a559-9c13d3794547"), new Guid("ec9607b4-eeb3-4fa2-bb21-0a728ced03f1") },
                    { new Guid("37fad974-167b-4d6f-9cc6-c57b488b72a7"), new Guid("ec9607b4-eeb3-4fa2-bb21-0a728ced03f1") },
                    { new Guid("3fc973af-c16f-4a92-a461-3cce8f5cecf9"), new Guid("ec9607b4-eeb3-4fa2-bb21-0a728ced03f1") },
                    { new Guid("6b8a28d1-d8eb-46d4-9946-b6007dbb7c23"), new Guid("ec9607b4-eeb3-4fa2-bb21-0a728ced03f1") }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("da91b68a-e3bf-4f88-8a72-382a9b868759"), new Guid("228d0a28-fe99-4c14-9e4c-d43b5feb02ed") },
                    { new Guid("76477f30-8761-49eb-94ce-5685af2112d6"), new Guid("60345af2-506f-45d9-bc6d-1e5d16a0e105") },
                    { new Guid("ec9607b4-eeb3-4fa2-bb21-0a728ced03f1"), new Guid("60345af2-506f-45d9-bc6d-1e5d16a0e105") },
                    { new Guid("da91b68a-e3bf-4f88-8a72-382a9b868759"), new Guid("929bfd3f-148a-4e2e-85d7-411de090ebfe") },
                    { new Guid("da91b68a-e3bf-4f88-8a72-382a9b868759"), new Guid("9f6bb990-edb9-41fc-8f5e-ec63fa3e4d3f") },
                    { new Guid("da91b68a-e3bf-4f88-8a72-382a9b868759"), new Guid("aa5bed02-852a-4a88-a107-159fbab169c2") },
                    { new Guid("da91b68a-e3bf-4f88-8a72-382a9b868759"), new Guid("cdd9d41a-161e-4dd5-a9f4-d521233f03b7") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
