using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenses.Domain.Migrations
{
    /// <inheritdoc />
    public partial class InitExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Balance");

            migrationBuilder.EnsureSchema(
                name: "Expenses");

            migrationBuilder.EnsureSchema(
                name: "Projects");

            migrationBuilder.EnsureSchema(
                name: "Categories");

            migrationBuilder.CreateTable(
                name: "Balances",
                schema: "Balance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrencyId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCategories",
                schema: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProjects",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                schema: "Expenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserCategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: true),
                    IsPositive = table.Column<bool>(type: "boolean", nullable: false),
                    UserProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_UserCategories_UserCategoryId",
                        column: x => x.UserCategoryId,
                        principalSchema: "Categories",
                        principalTable: "UserCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Expenses_UserProjects_UserProjectId",
                        column: x => x.UserProjectId,
                        principalSchema: "Projects",
                        principalTable: "UserProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAllowedProjects",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsReadOnly = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAllowedProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAllowedProjects_UserProjects_UserProjectId",
                        column: x => x.UserProjectId,
                        principalSchema: "Projects",
                        principalTable: "UserProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_UserCategoryId",
                schema: "Expenses",
                table: "Expenses",
                column: "UserCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_UserProjectId",
                schema: "Expenses",
                table: "Expenses",
                column: "UserProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAllowedProjects_UserProjectId",
                schema: "Projects",
                table: "UserAllowedProjects",
                column: "UserProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Balances",
                schema: "Balance");

            migrationBuilder.DropTable(
                name: "Expenses",
                schema: "Expenses");

            migrationBuilder.DropTable(
                name: "UserAllowedProjects",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "UserCategories",
                schema: "Categories");

            migrationBuilder.DropTable(
                name: "UserProjects",
                schema: "Projects");
        }
    }
}
