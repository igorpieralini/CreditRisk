using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreditRisk.RiskDecision.Migrations
{
    /// <inheritdoc />
    public partial class InitialRiskDecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "credit_decisions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreditOperationId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    RiskScore = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    GuaranteeCoveragePercentage = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    LimitUtilizationPercentage = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_decisions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "decision_policies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    MinimumRiskScore = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    MinimumGuaranteeCoverage = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    MaximumLimitUtilization = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_decision_policies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "decision_histories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreditDecisionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_decision_histories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_decision_histories_credit_decisions_CreditDecisionId",
                        column: x => x.CreditDecisionId,
                        principalTable: "credit_decisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_credit_decisions_CreatedAt",
                table: "credit_decisions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_credit_decisions_CreditOperationId",
                table: "credit_decisions",
                column: "CreditOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_decisions_Status",
                table: "credit_decisions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_decision_histories_CreatedAt",
                table: "decision_histories",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_decision_histories_CreditDecisionId",
                table: "decision_histories",
                column: "CreditDecisionId");

            migrationBuilder.CreateIndex(
                name: "IX_decision_policies_IsActive",
                table: "decision_policies",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_decision_policies_Name",
                table: "decision_policies",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "decision_histories");

            migrationBuilder.DropTable(
                name: "decision_policies");

            migrationBuilder.DropTable(
                name: "credit_decisions");
        }
    }
}
