using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace demo.SqliteRepositoryLib.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Functions",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<long>(nullable: false),
                    FuncCode = table.Column<string>(maxLength: 64, nullable: true),
                    FuncName = table.Column<string>(maxLength: 64, nullable: true),
                    Icon = table.Column<string>(maxLength: 64, nullable: true),
                    IsMenu = table.Column<bool>(nullable: false),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    OrderNo = table.Column<int>(nullable: false),
                    ParentFuncID = table.Column<long>(nullable: false),
                    UpdateUserID = table.Column<long>(nullable: false),
                    Url = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<long>(nullable: false),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    RoleName = table.Column<string>(maxLength: 32, nullable: true),
                    Type = table.Column<int>(nullable: false),
                    UpdateUserID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Alias = table.Column<string>(maxLength: 32, nullable: true),
                    Birthday = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<long>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Eyes = table.Column<decimal>(type: "decimal(12,4)", nullable: false),
                    Face = table.Column<string>(maxLength: 64, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    Lock = table.Column<bool>(nullable: false),
                    Memo = table.Column<string>(maxLength: 1024, nullable: true),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Sex = table.Column<int>(nullable: false),
                    Tel = table.Column<string>(maxLength: 32, nullable: true),
                    UpdateUserID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RoleFunction",
                columns: table => new
                {
                    RoleID = table.Column<long>(nullable: false),
                    FunctionID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleFunction", x => new { x.RoleID, x.FunctionID });
                    table.UniqueConstraint("AK_RoleFunction_FunctionID_RoleID", x => new { x.FunctionID, x.RoleID });
                    table.ForeignKey(
                        name: "FK_RoleFunction_Functions_FunctionID",
                        column: x => x.FunctionID,
                        principalTable: "Functions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleFunction_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserID = table.Column<long>(nullable: false),
                    RoleID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserID, x.RoleID });
                    table.UniqueConstraint("AK_UserRole_RoleID_UserID", x => new { x.RoleID, x.UserID });
                    table.ForeignKey(
                        name: "FK_UserRole_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleFunction");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Functions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
