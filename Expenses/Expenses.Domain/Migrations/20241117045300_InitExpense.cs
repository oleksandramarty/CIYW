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

            migrationBuilder.CreateTable(
                name: "UserProjects",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<string>(type: "character(32)", fixedLength: true, maxLength: 32, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Balances",
                schema: "Balance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrencyId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IconId = table.Column<int>(type: "integer", nullable: false),
                    UserProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserProjectId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<string>(type: "character(32)", fixedLength: true, maxLength: 32, nullable: false),
                    BalanceTypeId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Balances_UserProjects_UserProjectId",
                        column: x => x.UserProjectId,
                        principalSchema: "Projects",
                        principalTable: "UserProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Balances_UserProjects_UserProjectId1",
                        column: x => x.UserProjectId1,
                        principalSchema: "Projects",
                        principalTable: "UserProjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FavoriteExpenses",
                schema: "Expenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Limit = table.Column<decimal>(type: "numeric", nullable: true),
                    CurrentAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: true),
                    FrequencyId = table.Column<int>(type: "integer", nullable: true),
                    CurrencyId = table.Column<int>(type: "integer", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    IconId = table.Column<int>(type: "integer", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<string>(type: "character(32)", fixedLength: true, maxLength: 32, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteExpenses_UserProjects_UserProjectId",
                        column: x => x.UserProjectId,
                        principalSchema: "Projects",
                        principalTable: "UserProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlannedExpenses",
                schema: "Expenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    BalanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NextDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    FrequencyId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<string>(type: "character(32)", fixedLength: true, maxLength: 32, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannedExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlannedExpenses_UserProjects_UserProjectId",
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
                    IsReadOnly = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<string>(type: "character(32)", fixedLength: true, maxLength: 32, nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Expenses",
                schema: "Expenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    BalanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    UserProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<string>(type: "character(32)", fixedLength: true, maxLength: 32, nullable: false),
                    FavoriteExpenseId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_FavoriteExpenses_FavoriteExpenseId",
                        column: x => x.FavoriteExpenseId,
                        principalSchema: "Expenses",
                        principalTable: "FavoriteExpenses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Expenses_UserProjects_UserProjectId",
                        column: x => x.UserProjectId,
                        principalSchema: "Projects",
                        principalTable: "UserProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Balances_UserProjectId",
                schema: "Balance",
                table: "Balances",
                column: "UserProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Balances_UserProjectId1",
                schema: "Balance",
                table: "Balances",
                column: "UserProjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_FavoriteExpenseId",
                schema: "Expenses",
                table: "Expenses",
                column: "FavoriteExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_UserProjectId",
                schema: "Expenses",
                table: "Expenses",
                column: "UserProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteExpenses_UserProjectId",
                schema: "Expenses",
                table: "FavoriteExpenses",
                column: "UserProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedExpenses_UserProjectId",
                schema: "Expenses",
                table: "PlannedExpenses",
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
                name: "PlannedExpenses",
                schema: "Expenses");

            migrationBuilder.DropTable(
                name: "UserAllowedProjects",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "FavoriteExpenses",
                schema: "Expenses");

            migrationBuilder.DropTable(
                name: "UserProjects",
                schema: "Projects");
        }
    }
}
