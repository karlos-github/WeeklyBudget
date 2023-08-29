using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WeeklyBudget.Migrations
{
    /// <inheritdoc />
    public partial class initialxx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    BudgetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetDate = table.Column<DateTime>(type: "date", nullable: false),
                    TotalBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.BudgetId);
                });

            migrationBuilder.CreateTable(
                name: "ExpenditureTypes",
                columns: table => new
                {
                    ExpenditureTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenditureTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenditureTypes", x => x.ExpenditureTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Expenditures",
                columns: table => new
                {
                    ExpenditureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpentDate = table.Column<DateTime>(type: "date", nullable: false),
                    BudgetId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ExpenditureTypeId = table.Column<int>(type: "int", nullable: false),
                    SpentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenditures", x => x.ExpenditureId);
                    table.ForeignKey(
                        name: "FK_Expenditures_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budgets",
                        principalColumn: "BudgetId");
                });

            migrationBuilder.CreateTable(
                name: "BudgetDetails",
                columns: table => new
                {
                    BudgetDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetId = table.Column<int>(type: "int", nullable: false),
                    ExpenditureTypeId = table.Column<int>(type: "int", nullable: true),
                    TotalBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetDetails", x => x.BudgetDetailId);
                    table.ForeignKey(
                        name: "FK_BudgetDetails_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budgets",
                        principalColumn: "BudgetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetDetails_ExpenditureTypes_ExpenditureTypeId",
                        column: x => x.ExpenditureTypeId,
                        principalTable: "ExpenditureTypes",
                        principalColumn: "ExpenditureTypeId");
                });

            migrationBuilder.InsertData(
                table: "Budgets",
                columns: new[] { "BudgetId", "BudgetDate", "TotalBudget" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 7, 10, 17, 6, 36, 526, DateTimeKind.Local).AddTicks(1021), 18000.0m },
                    { 2, new DateTime(2023, 7, 10, 17, 6, 36, 526, DateTimeKind.Local).AddTicks(1069), 12000.0m }
                });

            migrationBuilder.InsertData(
                table: "ExpenditureTypes",
                columns: new[] { "ExpenditureTypeId", "ExpenditureTypeName" },
                values: new object[,]
                {
                    { 1, "Sex" },
                    { 2, "Drogy" },
                    { 3, "Alcohol" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "UserName" },
                values: new object[,]
                {
                    { 1, "Pepa" },
                    { 2, "Růžena" }
                });

            migrationBuilder.InsertData(
                table: "BudgetDetails",
                columns: new[] { "BudgetDetailId", "BudgetId", "ExpenditureTypeId", "TotalBudget" },
                values: new object[,]
                {
                    { 1, 1, 1, 2000.0m },
                    { 2, 1, 2, 2000.0m },
                    { 3, 1, 3, 4000.0m },
                    { 4, 2, 1, 3000.0m },
                    { 5, 2, 2, 2500.0m },
                    { 6, 2, 3, 5000.0m }
                });

            migrationBuilder.InsertData(
                table: "Expenditures",
                columns: new[] { "ExpenditureId", "BudgetId", "ExpenditureTypeId", "SpentAmount", "SpentDate", "UserId" },
                values: new object[,]
                {
                    { 1, 1, 1, 1000.0m, new DateTime(2023, 7, 10, 17, 6, 36, 526, DateTimeKind.Local).AddTicks(1882), 1 },
                    { 2, 1, 2, 2000.0m, new DateTime(2023, 7, 10, 17, 6, 36, 526, DateTimeKind.Local).AddTicks(1886), 1 },
                    { 3, 1, 1, 3000.0m, new DateTime(2023, 7, 10, 17, 6, 36, 526, DateTimeKind.Local).AddTicks(1889), 2 },
                    { 4, 2, 0, 1000.0m, new DateTime(2023, 7, 10, 17, 6, 36, 526, DateTimeKind.Local).AddTicks(1898), 1 },
                    { 5, 2, 3, 2000.0m, new DateTime(2023, 7, 10, 17, 6, 36, 526, DateTimeKind.Local).AddTicks(1901), 1 },
                    { 6, 2, 3, 3000.0m, new DateTime(2023, 7, 10, 17, 6, 36, 526, DateTimeKind.Local).AddTicks(1904), 2 },
                    { 7, 2, 2, 1000.0m, new DateTime(2023, 7, 10, 17, 6, 36, 526, DateTimeKind.Local).AddTicks(1907), 2 },
                    { 8, 2, 2, 100.0m, new DateTime(2023, 7, 10, 17, 6, 36, 526, DateTimeKind.Local).AddTicks(1909), 2 },
                    { 9, 2, 1, 150.0m, new DateTime(2023, 7, 10, 17, 6, 36, 526, DateTimeKind.Local).AddTicks(1912), 2 },
                    { 10, 1, 1, 100.0m, new DateTime(2023, 7, 10, 17, 6, 36, 526, DateTimeKind.Local).AddTicks(1892), 1 },
                    { 11, 1, 3, 400.0m, new DateTime(2023, 7, 10, 17, 6, 36, 526, DateTimeKind.Local).AddTicks(1895), 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_BudgetId",
                table: "BudgetDetails",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_ExpenditureTypeId",
                table: "BudgetDetails",
                column: "ExpenditureTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenditures_BudgetId",
                table: "Expenditures",
                column: "BudgetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetDetails");

            migrationBuilder.DropTable(
                name: "Expenditures");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ExpenditureTypes");

            migrationBuilder.DropTable(
                name: "Budgets");
        }
    }
}
