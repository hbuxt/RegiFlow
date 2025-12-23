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
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
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
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectUsers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectUserRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectUserRoles_ProjectUsers_ProjectUserId",
                        column: x => x.ProjectUserId,
                        principalTable: "ProjectUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectUserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("0e9b858f-164d-4c75-a559-9c13d3794547"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows the user to view their roles.", "user.roles.read" },
                    { new Guid("2ad51633-09b5-4abc-8cd7-0fef16ca08de"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows the user to delete a project.", "project.delete" },
                    { new Guid("37fad974-167b-4d6f-9cc6-c57b488b72a7"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows the user to update their details.", "user.update" },
                    { new Guid("3f396475-3e5a-4c44-93c0-77acc30e494f"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows the user to view their permissions.", "user.permissions.read" },
                    { new Guid("3fc973af-c16f-4a92-a461-3cce8f5cecf9"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows the user to delete their details.", "user.delete" },
                    { new Guid("6b8a28d1-d8eb-46d4-9946-b6007dbb7c23"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows the user to view their details.", "user.read" },
                    { new Guid("73fbc56a-be18-45d8-bb78-5fd8b0391b6c"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows the user to create projects.", "project.create" },
                    { new Guid("7b91219a-11ff-46c3-88b3-bd483c3a1658"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows the user to view project they're involved with.", "project.read" },
                    { new Guid("d132729b-1009-48d2-a6c1-17761c8ff500"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows the user to update a project.", "project.update" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Scope" },
                values: new object[,]
                {
                    { new Guid("b4d50721-7c41-491b-92d7-a8213599cc2b"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows the user to perform all project operations, including project deletion.", "Owner", 1 },
                    { new Guid("da91b68a-e3bf-4f88-8a72-382a9b868759"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A role assigned for demo accounts.", "Demo", 0 },
                    { new Guid("ec9607b4-eeb3-4fa2-bb21-0a728ced03f1"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The role assigned when registering for an account.", "General", 0 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Email", "FirstName", "HashedPassword", "IsDeleted", "LastName" },
                values: new object[] { new Guid("60345af2-506f-45d9-bc6d-1e5d16a0e105"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "h.buxton@wearewattle.com", "Harry", "C4F4270B5689DA8BE30D0CD9BFD6BB3FF18E9EA08EDAC567CB2FBCCAFB21B0B5-3B1D5C556AF69604D16EB2C3B5821AB8", false, "Buxton" });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("2ad51633-09b5-4abc-8cd7-0fef16ca08de"), new Guid("b4d50721-7c41-491b-92d7-a8213599cc2b") },
                    { new Guid("7b91219a-11ff-46c3-88b3-bd483c3a1658"), new Guid("b4d50721-7c41-491b-92d7-a8213599cc2b") },
                    { new Guid("d132729b-1009-48d2-a6c1-17761c8ff500"), new Guid("b4d50721-7c41-491b-92d7-a8213599cc2b") },
                    { new Guid("0e9b858f-164d-4c75-a559-9c13d3794547"), new Guid("da91b68a-e3bf-4f88-8a72-382a9b868759") },
                    { new Guid("3f396475-3e5a-4c44-93c0-77acc30e494f"), new Guid("da91b68a-e3bf-4f88-8a72-382a9b868759") },
                    { new Guid("6b8a28d1-d8eb-46d4-9946-b6007dbb7c23"), new Guid("da91b68a-e3bf-4f88-8a72-382a9b868759") },
                    { new Guid("7b91219a-11ff-46c3-88b3-bd483c3a1658"), new Guid("da91b68a-e3bf-4f88-8a72-382a9b868759") },
                    { new Guid("0e9b858f-164d-4c75-a559-9c13d3794547"), new Guid("ec9607b4-eeb3-4fa2-bb21-0a728ced03f1") },
                    { new Guid("37fad974-167b-4d6f-9cc6-c57b488b72a7"), new Guid("ec9607b4-eeb3-4fa2-bb21-0a728ced03f1") },
                    { new Guid("3f396475-3e5a-4c44-93c0-77acc30e494f"), new Guid("ec9607b4-eeb3-4fa2-bb21-0a728ced03f1") },
                    { new Guid("3fc973af-c16f-4a92-a461-3cce8f5cecf9"), new Guid("ec9607b4-eeb3-4fa2-bb21-0a728ced03f1") },
                    { new Guid("6b8a28d1-d8eb-46d4-9946-b6007dbb7c23"), new Guid("ec9607b4-eeb3-4fa2-bb21-0a728ced03f1") },
                    { new Guid("73fbc56a-be18-45d8-bb78-5fd8b0391b6c"), new Guid("ec9607b4-eeb3-4fa2-bb21-0a728ced03f1") },
                    { new Guid("7b91219a-11ff-46c3-88b3-bd483c3a1658"), new Guid("ec9607b4-eeb3-4fa2-bb21-0a728ced03f1") }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("ec9607b4-eeb3-4fa2-bb21-0a728ced03f1"), new Guid("60345af2-506f-45d9-bc6d-1e5d16a0e105") });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CreatedById",
                table: "Projects",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserRoles_ProjectUserId",
                table: "ProjectUserRoles",
                column: "ProjectUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserRoles_RoleId",
                table: "ProjectUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_ProjectId",
                table: "ProjectUsers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_UserId",
                table: "ProjectUsers",
                column: "UserId");

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
                name: "ProjectUserRoles");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "ProjectUsers");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
