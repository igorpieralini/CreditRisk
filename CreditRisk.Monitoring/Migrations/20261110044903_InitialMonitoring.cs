using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreditRisk.Monitoring.Migrations
{
    /// <inheritdoc />
    public partial class InitialMonitoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "credit_monitorings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreditOperationId = table.Column<Guid>(type: "uuid", nullable: false),
                    RiskScore = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    GuaranteeCoveragePercentage = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    LimitUtilizationPercentage = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_monitorings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "monitoring_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreditMonitoringId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_monitoring_events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_monitoring_events_credit_monitorings_CreditMonitoringId",
                        column: x => x.CreditMonitoringId,
                        principalTable: "credit_monitorings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "risk_alerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreditMonitoringId = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_risk_alerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_risk_alerts_credit_monitorings_CreditMonitoringId",
                        column: x => x.CreditMonitoringId,
                        principalTable: "credit_monitorings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_credit_monitorings_CreditOperationId",
                table: "credit_monitorings",
                column: "CreditOperationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_credit_monitorings_RiskScore",
                table: "credit_monitorings",
                column: "RiskScore");

            migrationBuilder.CreateIndex(
                name: "IX_monitoring_events_CreatedAt",
                table: "monitoring_events",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_monitoring_events_CreditMonitoringId",
                table: "monitoring_events",
                column: "CreditMonitoringId");

            migrationBuilder.CreateIndex(
                name: "IX_monitoring_events_Type",
                table: "monitoring_events",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_risk_alerts_CreatedAt",
                table: "risk_alerts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_risk_alerts_CreditMonitoringId",
                table: "risk_alerts",
                column: "CreditMonitoringId");

            migrationBuilder.CreateIndex(
                name: "IX_risk_alerts_Level",
                table: "risk_alerts",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_risk_alerts_Status",
                table: "risk_alerts",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "monitoring_events");

            migrationBuilder.DropTable(
                name: "risk_alerts");

            migrationBuilder.DropTable(
                name: "credit_monitorings");
        }
    }
}
