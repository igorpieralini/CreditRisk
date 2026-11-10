using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreditRisk.Guarantee.Migrations
{
    /// <inheritdoc />
    public partial class InitialGuarantee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "guarantees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    OriginalValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guarantees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "guarantee_operations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GuaranteeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreditOperationId = table.Column<Guid>(type: "uuid", nullable: false),
                    AllocatedValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    LinkedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReleasedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guarantee_operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_guarantee_operations_guarantees_GuaranteeId",
                        column: x => x.GuaranteeId,
                        principalTable: "guarantees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "guarantee_valuations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GuaranteeId = table.Column<Guid>(type: "uuid", nullable: false),
                    MarketValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    HaircutPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    EligibleValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValuationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guarantee_valuations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_guarantee_valuations_guarantees_GuaranteeId",
                        column: x => x.GuaranteeId,
                        principalTable: "guarantees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_guarantee_operations_CreditOperationId",
                table: "guarantee_operations",
                column: "CreditOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_guarantee_operations_GuaranteeId",
                table: "guarantee_operations",
                column: "GuaranteeId");

            migrationBuilder.CreateIndex(
                name: "IX_guarantee_operations_IsActive",
                table: "guarantee_operations",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_guarantee_valuations_GuaranteeId_ValuationDate",
                table: "guarantee_valuations",
                columns: new[] { "GuaranteeId", "ValuationDate" });

            migrationBuilder.CreateIndex(
                name: "IX_guarantees_IsActive",
                table: "guarantees",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_guarantees_Type",
                table: "guarantees",
                column: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "guarantee_operations");

            migrationBuilder.DropTable(
                name: "guarantee_valuations");

            migrationBuilder.DropTable(
                name: "guarantees");
        }
    }
}
